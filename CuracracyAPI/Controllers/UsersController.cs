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
	public class UserController : Controller {
		[HttpGet]
		public IEnumerable<string> Get() {
			return new string[] { "UserA", "UserB" };
		}
		
		[Route("register")]
		[Produces("application/json")]
		[HttpPost]
		public async Task<IActionResult> Register([FromForm]string username, [FromForm] string email, [FromForm] string password, [FromForm] string birthdate) {
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
						
						return Json(new {success = true, message = "Successfully created user."});
					} else {
						throw new Exception("ERROR: A user using this email already exists!");
					}
				}
			} catch (Exception e) {
				errorMessage = e.ToString();
			}
			return Json(new {success = false, message = errorMessage});
		}
			
		[Route("login")]
		[HttpPost]
		public IActionResult Login([FromForm]string email, [FromForm]string password, [FromForm]bool rememberMe) {
			string errorMessage = "";
			try {
				// Note: passwords are transmitted as plain text via SSL.  We do this primarily for noscript users.
				using (var db = new CuracracyContext()) {
					var user = db.AuthenticatedUsers.Where(x=> x.email == email).Include(u=> u.user);
					// Get the UserMeta from our query.
					AuthenticatedUser au = user.First();
					
					// If the user exists, continue with validation.
					if (au != null) {
						
						// Validate their password.
						bool valid = BCrypt.Net.BCrypt.Verify(password + au.salt, au.password);
						
						if (valid) {
							// If everything is good, issue a session token.
							string sessionToken = BCrypt.Net.BCrypt.HashString(email + DateTime.Now.ToString());
							
							db.Add(new AuthenticatedSession {user = au.user, sessionId = sessionToken, expirationDate = (rememberMe) ? DateTime.Today.Add(new TimeSpan(30, 0, 0, 0)) : DateTime.Today.Add( new TimeSpan(1, 0, 0, 0))});
							
							// We try to do a little house cleaning here for any expired tokens between logins.
							var tokens = db.AuthenticatedSessions.Where(t=> t.user.id == au.user.id).Include(u=> u.user);
							
							foreach (var t in tokens) {
								if (t.expirationDate < DateTime.Today) {
									db.Remove(t);
								}
							}
							
							db.SaveChanges();
							
							return Json(new {success = true, message = "Login successful.", sessionId = sessionToken, userId = au.user.id});
						} else {
							throw new Exception("Password is incorrect!");
						}
						
					} else {
						throw new Exception("User does not exist!");
					}
				}
			} catch (Exception e) {
				errorMessage = e.ToString();
			}
			
			return Json(new {success = false, message = errorMessage});
		}
		
		// TODO: Make token validation internal with no public API outside of supplying a token in a post request.
		[Route("validateToken")]
		[HttpPost]
		public IActionResult ValidateSessionToken([FromForm]string token, [FromForm]int userId) {
			string errorMessage = "";
			try {
				using (var db = new CuracracyContext()) {
					// Get all of our tokens.
					var tokenQuery = db.AuthenticatedSessions.Where(t=> t.user.id == userId);
					
					// Run the query and extract a list.
					var tokens = tokenQuery.ToList();
					
					// Validate the token.
					foreach (var t in tokens) {
						// We handle a little bit of house keeping down here as well.
						if (t.expirationDate < DateTime.Today) {
							// If the token we are presently checking is expired, remove it from the database.
							db.Remove(t);
							
							// If the token being validated is expired, boot the user out.
							if (t.sessionId == token) {
								db.SaveChanges();
								throw new Exception("Token expired!");
							}
						} else {
							// Success!  The token is valid.
							if (t.sessionId == token) {
								db.SaveChanges();
								return Json(new { success = true, message = "Token validation successful." });
							}
						}
						
						// Make sure any house cleaning is committed to the database.
						db.SaveChanges();
						throw new Exception("Token not found!");
					}
				}
			} catch (Exception e) {
				errorMessage = e.ToString();
			}
			
			return Json(new { success =  false, message = errorMessage });
		}
		
		[Route("{id}")]
		[HttpGet]
		public IActionResult GetUser(int id, [FromForm]string token) {
			string errorMessage = "";
			try {
				using (var db = new CuracracyContext()) {
					var userQuery = db.UserMetadata.Where(u=> u.id == id).Include(u=> u.userdata).Include(f=> f.userdata.galleryFolder.children);
					
					UserMeta user = userQuery.First();
					
					if (user != null) {
						return Json(new {
							success = true,
							user = new {
								name = user.userName,
								rank = user.rank,
								registered = user.registrationDate,
								description = user.userdata.description,
								favoritesFolder = user.userdata.galleryFolder.children.ElementAt(0), // Eventually these folder entries should probably list the x most recent entries instead of folder metadata.
								submissionsFolder = user.userdata.galleryFolder.children.ElementAt(1),
								shoutThread = user.userdata.shoutsThreadId
							}
						});
					} else {
						throw new Exception("User not found!");
					}
				}
			} catch (Exception e) {
				errorMessage = e.ToString();
			}
			return Json(new { success = false, message = errorMessage});
		}
	}
}
