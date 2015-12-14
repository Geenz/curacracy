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
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
        
        public async Task<IActionResult> Login() {
            Console.WriteLine("Getting context.");
            if (HttpContext.Session != null) {
                Console.WriteLine("Getting session info.");
                var userID = HttpContext.Session.GetInt32("userid");
                var userSession = HttpContext.Session.GetString("token");
                if (userID != null && userSession != null) {
                    using (var client = new HttpClient()) {
                        Console.WriteLine("Requesting validation for {0} with token {1}", userID.ToString(), userSession.ToString());
                        
                        client.BaseAddress = new Uri("http://localhost:5000");
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        
                        var formValues = new List<KeyValuePair<string, string>>();
                        formValues.Add(new KeyValuePair<string, string>("userId", userID.ToString()));
                        formValues.Add(new KeyValuePair<string, string>("token", userSession.ToString()));
                        
                        var response = await client.PostAsync("http://localhost:5000/api/v1/user/validateToken", new FormUrlEncodedContent(formValues));
                        Stream receiveStream = await response.Content.ReadAsStreamAsync();
                        StreamReader readStream = new StreamReader (receiveStream, Encoding.UTF8);
                        Console.WriteLine(readStream.ReadToEnd());
                    }
                }
            }
            
            ViewData["LoggedIn"] = false;
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> Login([FromForm]string email, [FromForm]string password)
        {
            Console.WriteLine(email + ", " + password);
            using (var client = new HttpClient()) {
                client.BaseAddress = new Uri("http://localhost:5000");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                
                var formValues = new List<KeyValuePair<string, string>>();
                formValues.Add(new KeyValuePair<string, string>("email", email));
                formValues.Add(new KeyValuePair<string, string>("password", password));
                formValues.Add(new KeyValuePair<string, string>("rememberMe", "false"));
                
                var response = await client.PostAsync("http://localhost:5000/api/v1/user/login", new FormUrlEncodedContent(formValues));
                
                
                Stream receiveStream = await response.Content.ReadAsStreamAsync();
                StreamReader readStream = new StreamReader (receiveStream, Encoding.UTF8);
                UserSession u = JsonConvert.DeserializeObject<UserSession>(readStream.ReadToEnd());
                Console.WriteLine(readStream.ReadToEnd());
                ViewData["LoggedIn"] = true;
                
                HttpContext.Session.SetInt32("userid", u.userid);
                HttpContext.Session.SetString("token", u.token);
            }
            return View();
        }
    }
    
    public class UserSession {
        public int userid {get; set;}
        
        public string token {get; set;}
    }
}
