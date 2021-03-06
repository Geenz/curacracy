using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.Data.Entity;
using CuracracyAPI.Models;
using Newtonsoft.Json;

namespace CuracracyAPI.Controllers {
	[Route("api/v1/[controller]")]
	public class UserController : Controller {
		[Route("page/{id}")]
		[HttpGet]
		public IEnumerable<UserMeta> GetPage(int id) {
			string errorMessage = "";
			try {
				using (var db = new CuracracyContext()) {
					
					var userQuery = db.UserMetadata.Skip(id * 50).Take(50);
					
					// Execute the query.
					var userMetadata = userQuery.ToList();
					
					if (userMetadata.Any()) {
						// This endpoint works in pages, so return 206 if we have anything.
						HttpContext.Response.StatusCode = 206;
						HttpContext.Response.Headers["Content-Range"] = (id * 50).ToString() + "-" + (id * 50 + userMetadata.Count()).ToString();
					} else {
						// We don't have any results.  Return 204.
						HttpContext.Response.StatusCode = 204;
						return null;
					}

					return userMetadata;
				}
			} catch (Exception e) {
				errorMessage = e.ToString();
			}
			
			HttpContext.Response.StatusCode = 500;
			return null;
		}
		
		[Route("page/{id}")]
		[HttpPost]
		public IEnumerable<UserMeta> GetPage(int id, [FromForm]int count) {
			string errorMessage = "";
			try {
				using (var db = new CuracracyContext()) {
					
					var userQuery = db.UserMetadata.Skip(id * count).Take(count);
					
					// Execute the query.
					var userMetadata = userQuery.ToList();
					
					if (userMetadata.Any()) {
						// This endpoint works in pages, so return 206 if we have anything.
						HttpContext.Response.StatusCode = 206;
						HttpContext.Response.Headers["Content-Range"] = (id * count).ToString() + "-" + (id * count + userMetadata.Count()).ToString();
					} else {
						// We don't have any results.  Return 204.
						HttpContext.Response.StatusCode = 204;
						return null;
					}

					return userMetadata;
				}
			} catch (Exception e) {
				errorMessage = e.ToString();
			}
			
			HttpContext.Response.StatusCode = 500;
			return null;
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
		public async Task<UserResponse> UpdateUser(int id, [FromForm]string userdata) {
			string errorMessage = "";
            UserRequest ur = JsonConvert.DeserializeObject<UserRequest>(userdata);
            
			try {
				using (var db = new CuracracyContext()) {
					// First, validate the user token.
					ValidationResponse v = AuthenticationController.ValidateSession(ur.authtoken, ur.authid);
					if (v.validated) {
						// Next, update our user data.
						var userQuery = db.UserMetadata.Where(u=> u.id == id).Include(u=> u.userdata).Include(f=> f.userdata.galleryFolder.children);
						
						UserMeta user = userQuery.First();
						
						user.userName = ur.username;
						user.userdata.description = ur.description;
						
						await db.SaveChangesAsync();
						
						if (user != null) {
                            HttpContext.Response.StatusCode = 204;
							return new UserResponse(user);
						} else {
                            HttpContext.Response.StatusCode = 404;
							throw new Exception("User not found!");
						}
					}
				}
			} catch (Exception e) {
				errorMessage = e.ToString();
                HttpContext.Response.StatusCode = 500;
			}
			return null;
		}
	}
}
