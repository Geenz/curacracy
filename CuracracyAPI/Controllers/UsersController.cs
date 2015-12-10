using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
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
		public async Task<string> Register([FromForm]string username, [FromForm] string email, [FromForm] string password, [FromForm] string birthdate) {
			string errorMessage = "";
			try {
				using (var db = new CuracracyAPI.Models.CuracracyContext()) {
					// First, see if this user has registered before by email.
					email = email.Trim(' ');
					var user = from u in db.UserMetadata where u.email.Equals(email) select u;
					// If user is == 0, then we assume that they don't exist.
					if (!user.Any()) {
						// Create that user's root folder.
						Folder rootFolder = new Folder { name = username + "'s root folder"};
						db.Add(rootFolder);
						var count = await db.SaveChangesAsync();
						long rootId = rootFolder.id;
						
						// Create the other folders.
						db.Add(new CuracracyAPI.Models.Folder {name = username + "'s favorites", parentId = rootId});
						db.Add(new CuracracyAPI.Models.Folder {name = username + "'s submissions", parentId = rootId});
						
						// Create a new user.
						DateTime birthday;
						DateTime.TryParseExact(birthdate, "MM-DD-YYYY", null, DateTimeStyles.None, out birthday);
						
						// All passwords are salted before hashing.
						string salt = BCrypt.Net.BCrypt.GenerateSalt(10);
						
						db.Add(new CuracracyAPI.Models.UserMeta {email = email, registrationDate = DateTime.Now, userName = username, rank = 0, birthDate = birthday});
						db.Add(new CuracracyAPI.Models.User {galleryFolderId = rootId, shoutsThreadId = 0, description = ""});
						db.Add(new CuracracyAPI.Models.AuthenticatedUser {salt = salt, password = password + salt});
						count += await db.SaveChangesAsync();
						
						return "{\"success\": true, \"message\": \"Success\"}";
					} else {
						throw new Exception("ERROR: A user using this email already exists!");
					}
				}
			} catch (Exception e) {
				errorMessage = e.ToString();
			}
			return "{\"success\": false, \"message\": \"" + errorMessage + "\"}";
		}
			
		[Route("login")]
		[HttpPost]
		public string Login([FromForm]string email, [FromForm]string password, [FromForm]bool rememberMe) {
			string errorMessage = "";
			try {
				// Note: passwords are transmitted as plain text via SSL.  We do this primarily for noscript users.
				using (var db = new CuracracyAPI.Models.CuracracyContext()) {
					var user = db.UserMetadata.Where(x=> x.email == email);
					// Get the UserMeta from our query.
					UserMeta um = user.First();
					
					// If the user exists, continue with validation.
					if (um != null) {
						// Get their credentials.
						var userCred = from c in db.AuthenticatedUsers where c.id == um.id select c;
						
						// Execute the userCred query, and get our user's info.
						AuthenticatedUser au = userCred.First();
						
						// Validate their password.
						bool valid = BCrypt.Net.BCrypt.Verify(password + au.salt, au.password);
						
						if (valid) {
							// If everything is good, issue a session token.
							string sessionToken = BCrypt.Net.BCrypt.HashString(email + DateTime.Now.ToString());
							
							db.Add(new CuracracyAPI.Models.AuthenticatedSession {userid = um.id, sessionId = sessionToken, expirationDate = (rememberMe) ? DateTime.Today.Add(new TimeSpan(30, 0, 0, 0)) : DateTime.Today.Add( new TimeSpan(1, 0, 0, 0))});
							
							// We try to do a little house cleaning here for any expired tokens between logins.
							var tokens = from t in db.AuthenticatedSessions where t.userid == um.id select t;
							
							foreach (var t in tokens) {
								if (t.expirationDate > DateTime.Today) {
									db.Remove(t);
								}
							}
							
							db.SaveChanges();
							
							return "{\"success\": true, \"message\": \"Login successful.\", \"sessionid\": \"" + sessionToken + "\", \"userid\":" + um.id + "}";
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
			
			return "{\"success\": false, \"message\": \"" + errorMessage + "\"}";
		}
		
		[Route("validateToken")]
		[HttpPost]
		public string ValidateSessionToken([FromForm]string token, [FromForm]int userId) {
			string errorMessage = "";
			try {
				using (var db = new CuracracyAPI.Models.CuracracyContext()) {
					// Get all of our tokens.
					var tokenQuery = from t in db.AuthenticatedSessions where t.userid == userId select t;
					
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
								return "{\"success\": true}";
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
			
			return "{\"success\": false, \"message\": \"" + errorMessage + "\"}";
		}
		
		[Route("{id}")]
		[HttpGet]
		public string GetUser() {
			return "";
		}
	}
}
