using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.Data.Entity;
using CuracracyAPI.Models;

namespace CuracracyAPI.Controllers {
	[Route("api/v1/[controller]")]
	public class UserController : Controller {
		[HttpGet]
		public IEnumerable<string> Get() {
			return new string[] { "UserA", "UserB" };
		}
		
		[Route("{id}")]
		[HttpGet]
		public UserResponse GetUser(int id) {
			string errorMessage = "";
			try {
				using (var db = new CuracracyContext()) {
					var userQuery = db.UserMetadata.Where(u=> u.id == id).Include(u=> u.userdata).Include(f=> f.userdata.galleryFolder.children);
					
					UserMeta user = userQuery.First();
					
					if (user != null) {
						return new UserResponse(user);
					} else {
						throw new Exception("User not found!");
					}
				}
			} catch (Exception e) {
				errorMessage = e.ToString();
			}
			return null;
		}
		
		[Route("{id}")]
		[HttpPost]
		public UserResponse GetUser(int id, [FromForm]string token, [FromForm]int userid) {
			string errorMessage = "";
			try {
				using (var db = new CuracracyContext()) {
					var userQuery = db.UserMetadata.Where(u=> u.id == id).Include(u=> u.userdata).Include(f=> f.userdata.galleryFolder.children);
					
					UserMeta user = userQuery.First();
					
					if (user != null) {
						return new UserResponse(user);
					} else {
						throw new Exception("User not found!");
					}
				}
			} catch (Exception e) {
				errorMessage = e.ToString();
			}
			return null;
		}
		
		[Route("{id}/update")]
		[HttpPost]
		public async Task<UserResponse> UpdateUser(int id, [FromForm]string token, [FromForm]int userid, [FromForm]UserResponse userData) {
			string errorMessage = "";
			try {
				using (var db = new CuracracyContext()) {
					// First, validate the user token.
					ValidationResponse v = AuthenticationController.ValidateSession(token, userid);
					if (v.validated) {
						// Next, update our user data.
						var userQuery = db.UserMetadata.Where(u=> u.id == id).Include(u=> u.userdata).Include(f=> f.userdata.galleryFolder.children);
						
						UserMeta user = userQuery.First();
						
						user.userName = userData.username;
						user.userdata.description = userData.description;
						
						await db.SaveChangesAsync();
						
						if (user != null) {
							return new UserResponse(user);
						} else {
							throw new Exception("User not found!");
						}
					}
				}
			} catch (Exception e) {
				errorMessage = e.ToString();
			}
			return null;
		}
	}
}
