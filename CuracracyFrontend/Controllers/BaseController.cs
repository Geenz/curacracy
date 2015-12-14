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
    public class BaseController : Controller
    {   
        protected async Task<TokenResponse> LoggedIn() {
             // Session shouldn't be null, but just in case there's a case that I can't think of..
            if (HttpContext.Session != null) {
                var userID = HttpContext.Session.GetInt32("userid");
                var userSession = HttpContext.Session.GetString("token");
                
                // If userID and userSession are present, validate the session token.
                if (userID != null && userSession != null) {
                    using (var client = new HttpClient()) {
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        
                        var formValues = new List<KeyValuePair<string, string>>();
                        formValues.Add(new KeyValuePair<string, string>("userId", userID.ToString()));
                        formValues.Add(new KeyValuePair<string, string>("token", userSession.ToString()));
                        
                        var response = await client.PostAsync("http://localhost:5000/api/v1/authentication/validateToken", new FormUrlEncodedContent(formValues));
                        Stream receiveStream = await response.Content.ReadAsStreamAsync();
                        StreamReader readStream = new StreamReader (receiveStream, Encoding.UTF8);
                        
                        TokenResponse t = JsonConvert.DeserializeObject<TokenResponse>(readStream.ReadToEnd());
                        
                        return t;
                    }
                }
            }
            
            return new TokenResponse {validated = false};
        }
    }
    
    public class UserSession {
        public int userid {get; set;}
        
        public string token {get; set;}
    }
    
    public class TokenResponse {
        public bool validated {get; set;}
        
        public string reason {get; set;}
    }
}
