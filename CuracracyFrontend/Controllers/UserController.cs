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
    public class UserController : BaseController
    {
        public async Task<IActionResult> Index()
        {
            var log = await LoggedIn();
            ViewData["LoggedIn"] = log.validated;
            ViewData["Users"] = CuracracyAPI.Client.User.GetPage(0);
            return View();
        }
    }
}
