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

namespace CuracracyFrontend.Controllers
{
    public class AuthController : BaseController
    {
        public async Task<IActionResult> Logout() {
            // Session shouldn't be null, but just in case there's a case that I can't think of..
            if ((await LoggedIn()).validated) {
                var userID = HttpContext.Session.GetInt32("userid");
                var userSession = HttpContext.Session.GetString("token");
                
                // If userID and userSession are present, invalidate the session token.
                if (userID != null && userSession != null) {
                    using (var client = new HttpClient()) {
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        
                        var formValues = new List<KeyValuePair<string, string>>();
                        formValues.Add(new KeyValuePair<string, string>("userId", userID.ToString()));
                        formValues.Add(new KeyValuePair<string, string>("token", userSession.ToString()));
                        
                        var response = await client.PostAsync("http://localhost:5000/api/v1/authentication/invalidateToken", new FormUrlEncodedContent(formValues));
                        Stream receiveStream = await response.Content.ReadAsStreamAsync();
                        StreamReader readStream = new StreamReader (receiveStream, Encoding.UTF8);
                        
                        TokenResponse t = JsonConvert.DeserializeObject<TokenResponse>(readStream.ReadToEnd());
                        
                        ViewData["LoggedIn"] = false;
                        
                        if (t.validated) {
                            // Success!  We have been successfully logged out.
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
        
        public async Task<IActionResult> Login() {
            ViewData["LoggedIn"] = (await LoggedIn()).validated;
            
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> Login([FromForm]string email, [FromForm]string password)
        {
            using (var client = new HttpClient()) {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                
                var formValues = new List<KeyValuePair<string, string>>();
                formValues.Add(new KeyValuePair<string, string>("email", email));
                formValues.Add(new KeyValuePair<string, string>("password", password));
                formValues.Add(new KeyValuePair<string, string>("rememberMe", "false"));
                
                var response = await client.PostAsync("http://localhost:5000/api/v1/authentication/login", new FormUrlEncodedContent(formValues));
                
                if (response.IsSuccessStatusCode) {
                    Stream receiveStream = await response.Content.ReadAsStreamAsync();
                    StreamReader readStream = new StreamReader (receiveStream, Encoding.UTF8);
                    UserSession u = JsonConvert.DeserializeObject<UserSession>(readStream.ReadToEnd());
                    
                    ViewData["LoggedIn"] = true;
                    
                    // Set their session with their user ID and session token.
                    HttpContext.Session.SetInt32("userid", u.userid);
                    HttpContext.Session.SetString("token", u.token);
                    
                    // Redirect them to the home page.
                    // In the future, this needs to redirect to their previous location.
                    HttpContext.Response.Redirect("/");
                } else {
                    ViewData["Message"] = "Error: Username or password is incorrect! Check your credentials and try again.";
                    ViewData["LoggedIn"] = false;
                }
            }
            return View();
        }
        
        public async Task<IActionResult> Register() {
            var log = await LoggedIn();
            if (log.validated) {
                ViewData["LoggedIn"] = true;
            } else {
                ViewData["loggedIn"] = false;
            }
            
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> Register([FromForm]string email, [FromForm]string password, [FromForm]string username, [FromForm]string birthdate) {
            var log = await LoggedIn();
            if (log.validated) {
                ViewData["LoggedIn"] = true;
            } else {
                using (var client = new HttpClient()) {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    
                    var formValues = new List<KeyValuePair<string, string>>();
                    formValues.Add(new KeyValuePair<string, string>("username", email));
                    formValues.Add(new KeyValuePair<string, string>("email", email));
                    formValues.Add(new KeyValuePair<string, string>("password", password));
                    formValues.Add(new KeyValuePair<string, string>("birthdate", birthdate));
                    
                    var response = await client.PostAsync("http://localhost:5000/api/v1/authentication/register", new FormUrlEncodedContent(formValues));
                    
                    if (response.IsSuccessStatusCode) {
                        // Redirect them to the home page.
                        // In the future, this needs to redirect to their previous location.
                        HttpContext.Response.Redirect("/Auth/Login");
                    } else {
                        ViewData["Message"] = "Error: User already exists!";
                        ViewData["LoggedIn"] = false;
                    }
                }
            }
            return View();
        }
    }
    
}
