using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.Data.Entity;
using BCrypt.Net;
using CuracracyAPI.Models;

namespace CuracracyAPI.Controllers {
	[Route("api/v1/[controller]")]
	public class AuthenticationController : Controller {
		
		[Route("register")]
		[Produces("application/json")]
		[HttpPost]
		public async Task<GenericReponse> Register([FromForm]string username, [FromForm] string email, [FromForm] string password, [FromForm] string birthdate) {
			string errorMessage = "";
			try {
				using (var db = new CuracracyContext()) {
					// First, see if this user has registered before by email.
					email = email.Trim(' ');
					var user = db.AuthenticatedUsers.Where(u=>u.email == email);
					// If user is == 0, then we assume that they don't exist.
					if (!user.Any()) {
						// Create that user's root folder.
						Folder rootFolder = new Folder { name = username + "'s root folder"};
						rootFolder.children = new List<Folder>();
						rootFolder.children.Add(new Folder { name = username + "'s Favorites"});
						rootFolder.children.Add(new Folder { name = username + "'s Submissions"});
						db.Add(rootFolder);
						
						// Create a new user.
						DateTime birthday;
						DateTime.TryParseExact(birthdate, "MM-DD-YYYY", null, DateTimeStyles.None, out birthday);
						User u = new User { metadata = new UserMeta {registrationDate = DateTime.Now, userName = username, rank = 0, birthDate = birthday}, galleryFolder = rootFolder, shoutsThreadId = 0, description = ""};
						db.Add(u);
						
						// All passwords are salted before hashing.
						string salt = BCrypt.Net.BCrypt.GenerateSalt(10);
						db.Add(new AuthenticatedUser {salt = salt, password = password + salt, email = email, hashMethod = "bcrypt", user = u.metadata});
						
						await db.SaveChangesAsync();
						
						return new GenericReponse(true, "Successfully created user.");
					} else {
						throw new Exception("ERROR: A user using this email already exists!");
					}
				}
			} catch (Exception e) {
				errorMessage = e.ToString();
			}
			return new GenericReponse(false, errorMessage);
		}
			
		[Route("login")]
		[HttpPost]
		public LoginResponse Login([FromForm]string email, [FromForm]string password, [FromForm]bool rememberMe) {
			string errorMessage = "";
			try {
				// Note: passwords are transmitted as plain text via SSL.  We do this primarily for noscript users.
				using (var db = new CuracracyContext()) {
					// Construct our query.
					var user = db.AuthenticatedUsers.Where(x=> x.email == email).Include(u=> u.user);
					
					// Get the first user that gets returned (there shouldn't be any others anyways)
					AuthenticatedUser au = user.First();
					
					// If the user exists, continue with validation.
					if (au != null) {
						
						// Validate their password.
						bool valid = BCrypt.Net.BCrypt.Verify(password + au.salt, au.password);
						
						if (valid) {
							// If everything is good, issue a session token.
							string sessionToken = BCrypt.Net.BCrypt.HashString(email + DateTime.Now.ToString());
							AuthenticatedSession session = new AuthenticatedSession {user = au.user, sessionId = sessionToken, expirationDate = (rememberMe) ? DateTime.Today.Add(new TimeSpan(30, 0, 0, 0)) : DateTime.Today.Add( new TimeSpan(1, 0, 0, 0))}; 
							db.Add(session);
							
							// We try to do a little house cleaning here for any expired tokens between logins.
							var tokens = db.AuthenticatedSessions.Where(t=> t.user.id == au.user.id).Include(u=> u.user);
							
							foreach (var t in tokens) {
								if (t.expirationDate < DateTime.Today) {
									db.Remove(t);
								}
							}
							
							db.SaveChanges();
							HttpContext.Response.StatusCode = 200;
							
							var l = new LoginResponse(session.sessionId, session.user.id);
							Console.WriteLine("Authorizing with {0} and {1}", l.userid, l.token);
							return l;
						} else {
							HttpContext.Response.StatusCode = 401;
							throw new Exception("Password is incorrect!");
						}
						
					} else {
						HttpContext.Response.StatusCode = 401;
						throw new Exception("User does not exist!");
					}
				}
			} catch (Exception e) {
				errorMessage = e.ToString();
			}
			
			HttpContext.Response.StatusCode = 401;
			
			return null;
		}
		
		// TODO: Make token validation internal with no public API outside of supplying a token in a post request.
		[Route("validateToken")]
		[HttpPost]
		public ValidationResponse ValidateSessionToken([FromForm]string token, [FromForm]int userId) {
			ValidationResponse v = ValidateSession(token, userId);
			
			if (v.validated) {
				HttpContext.Response.StatusCode = 200;
			} else {
				HttpContext.Response.StatusCode = 401;
			}
			
			return v;
		}
		
		public static ValidationResponse ValidateSession([FromForm]string token, [FromForm]int userId) {
			token = token.Trim(' ');
			string errorMessage = "";
			try {
				using (var db = new CuracracyContext()) {
					// Get all of our tokens.
					var tokenQuery = db.AuthenticatedSessions.Where(t=> t.user.id == userId && t.sessionId == token);
					
					// Run the query and extract a list.
					var tokens = tokenQuery.ToList();
					
					// Validate the token.
					foreach (var t in tokens) {
						// We handle a little bit of house keeping down here as well.
						if (t.expirationDate < DateTime.Today) {
							// If the token we are presently checking is expired, remove it from the database.
							db.Remove(t);
						} else {
							// Success!  The token is valid.
							db.SaveChanges();
							return new ValidationResponse(true, "");
						}
					}
					// We should only get to this point if the database didn't return any results.
					
					// Make sure any house cleaning is committed to the database.
					db.SaveChanges();
					throw new Exception("Token not found!");
				}
			} catch (Exception e) {
				errorMessage = e.ToString();
			}
			
			return new ValidationResponse(false, errorMessage);
		}
		
		[RouteAttribute("invalidateToken")]
		[HttpPost]
		public ValidationResponse InvalidateSessionToken([FromForm]string token, [FromForm]int userId) {
			token = token.Trim(' ');
			string errorMessage = "";
			try {
				using (var db = new CuracracyContext()) {
					var tokenQuery = db.AuthenticatedSessions.Where(t=> t.user.id == userId && t.sessionId == token);
					
					var tokens = tokenQuery.ToList();
					
					foreach (var t in tokens) {
						db.Remove(t);
					}
					
					db.SaveChanges();
					return new ValidationResponse(true, "Token successfully invalidated.");
				}
			} catch (Exception e) {
				errorMessage = e.ToString();
			}
			
			return new ValidationResponse(false, errorMessage);
		}
	}
}
