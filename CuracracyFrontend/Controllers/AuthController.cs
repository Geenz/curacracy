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
using CuracracyAPI.Client;
using CuracracyAPI.Models;

namespace CuracracyFrontend.Controllers
{
    public class AuthController : BaseController
    {
        [Route("/logout")]
        public async Task<IActionResult> Logout() {
            // Session shouldn't be null, but just in case there's a case that I can't think of..
            if ((await LoggedIn()).validated) {
                var userID = HttpContext.Session.GetInt32(USER_ID_VAR);
                var userSession = HttpContext.Session.GetString(USER_TOKEN_VAR);
                
                // If userID and userSession are present, invalidate the session token.
                if (userID != null && userSession != null) {
                    ValidationResponse v = await Authentication.InvalidateTokenRequest(userSession, userID.Value);
                    if (v != null) {
                        if (v.validated) {
                            ViewData["LoggedIn"] = false;
                            HttpContext.Session.Clear();
                        }
                    }
                } else {
                    ViewData["LoggedIn"] = false;
                }
            } else {
                ViewData["LoggedIn"] = false;
            }
            
            return View();
        }
        
        [Route("/login")]
        public async Task<IActionResult> Login() {
            ViewData["LoggedIn"] = (await LoggedIn()).validated;
            
            return View();
        }
        
        [Route("/login")]
        [HttpPost]
        public async Task<IActionResult> Login([FromForm]string email, [FromForm]string password)
        {
            LoginResponse l = await Authentication.LoginRequest(email, password, false);
            
            if (l != null) {
                ViewData["LoggedIn"] = true;
                HttpContext.Session.SetInt32(USER_ID_VAR, l.userid);
                HttpContext.Session.SetString(USER_TOKEN_VAR, l.token);
                HttpContext.Response.Redirect("/");
            } else {
                ViewData["Message"] = "Error: Username or password is incorrect! Check your credentials and try again.";
                ViewData["LoggedIn"] = false;
            }
            return View();
        }
        
        [Route("/register")]
        public async Task<IActionResult> Register() {
            var log = await LoggedIn();
            if (log.validated) {
                ViewData["LoggedIn"] = true;
            } else {
                ViewData["loggedIn"] = false;
            }
            
            return View();
        }
        
        [Route("/register")]
        [HttpPost]
        public async Task<IActionResult> Register([FromForm]string email, [FromForm]string password, [FromForm]string username, [FromForm]string birthdate) {
            var log = await LoggedIn();
            if (log.validated) {
                ViewData["LoggedIn"] = true;
            } else {
                GenericReponse g = await Authentication.RegisterUserRequest(username, email, password, DateTime.Now);
                
                if (g != null) {
                    HttpContext.Response.Redirect("/Auth/Login");
                    ViewData["LoggedIn"] = false;
                } else {
                    ViewData["Message"] = "Error: User already exists!";
                    ViewData["LoggedIn"] = false;
                }
            }
            return View();
        }
    }
    
}
