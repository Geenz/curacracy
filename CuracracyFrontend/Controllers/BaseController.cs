using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Session;
using CuracracyAPI.Client;
using CuracracyAPI.Models;

namespace CuracracyFrontend.Controllers
{
    public class BaseController : Controller
    {
        public const string USER_ID_VAR = "userid";
        public const string USER_TOKEN_VAR = "token";
        protected async Task<ValidationResponse> LoggedIn() {
             // Session shouldn't be null, but just in case there's a case that I can't think of..
            if (HttpContext.Session != null) {
                var userID = HttpContext.Session.GetInt32(USER_ID_VAR);
                var userSession = HttpContext.Session.GetString(USER_TOKEN_VAR);
                
                if (userID.HasValue) {
                    ValidationResponse v = await Authentication.ValidateTokenRequest(userSession, userID.Value);
                    return v;
                }
            }
            
            return new ValidationResponse(false, "");
        }
    }
}
