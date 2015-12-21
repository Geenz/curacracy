using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Session;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using CuracracyAPI.Models;
using CuracracyAPI.Client;

namespace CuracracyFrontend.Controllers
{
    [Route("user")]
    public class UserController : BaseController
    {
        [Route("{id}")]
        public async Task<IActionResult> GetUser(int id) {
            var log = await LoggedIn();
            ViewData["LoggedIn"] = log.validated;
            
            int? suid = HttpContext.Session.GetInt32("userid");
            string sid = HttpContext.Session.GetString("token");
            
            UserResponse user;
            
            if (suid.HasValue) {
                user = await CuracracyAPI.Client.User.GetUser(id, new LoginResponse(sid, suid.Value));
            } else {
                user = await CuracracyAPI.Client.User.GetUser(id);
            }
            ViewData["Userdata"] = user;
            return View();
        }
        
        [Route("{id}/update")]
        [HttpGet]
        public async Task<IActionResult> EditUser(int id) {
            var log = await LoggedIn();
            ViewData["LoggedIn"] = log.validated;
            
            int? suid = HttpContext.Session.GetInt32("userid");
            string sid = HttpContext.Session.GetString("token");
            
            GenericResponse response;
            
            var user = await CuracracyAPI.Client.User.GetUser(id);
            
            if (!suid.HasValue) {
                response = new GenericResponse(false, "Unauthorized!");
            } else {
                response = new GenericResponse(true, "");
            }
            
            ViewData["Message"] = response.message;
            ViewData["Userdata"] = user;
            return View();
        }
        
        [Route("{id}/update")]
        [HttpPost]
        public async Task<IActionResult> EditUser(int id, [FromForm]string username, [FromForm]string description) {
            var log = await LoggedIn();
            ViewData["LoggedIn"] = log.validated;
            
            int? suid = HttpContext.Session.GetInt32("userid");
            string sid = HttpContext.Session.GetString("token");
            
            GenericResponse response;
            UserResponse user = null;
            if (suid.HasValue) {
                user = await CuracracyAPI.Client.User.GetUser(id, new LoginResponse(sid, suid.Value));
                user.username = username;
                user.description = description;
                response = await CuracracyAPI.Client.User.UpdateUser(user, new LoginResponse(sid, suid.Value));
                Console.WriteLine("Redirecting.");
                HttpContext.Response.Redirect("/user/" + id.ToString());
            } else {
                response = new GenericResponse(false, "Unauthorized!");
            }
            
            ViewData["Message"] = response.message;
            ViewData["Userdata"] = user;
            
            return View();
        }
        
        [Route("page/{pageNumber:int=0}")]
        public async Task<IActionResult> Index(int pageNumber) {
            var log = await LoggedIn();
            ViewData["LoggedIn"] = log.validated;
            var users = await CuracracyAPI.Client.User.GetUsers(pageNumber);
            ViewData["Users"] = users;
            return View();
        }
    }
}
