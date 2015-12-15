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
