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
    public class HomeController : BaseController
    {
        public async Task<IActionResult> Index()
        {
            var log = await LoggedIn();
            ViewData["LoggedIn"] = log.validated;
            return View();
        }

        public async Task<IActionResult> About()
        {
            var log = await LoggedIn();
            ViewData["LoggedIn"] = log.validated;
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public async Task<IActionResult> Contact()
        {
            var log = await LoggedIn();
            ViewData["LoggedIn"] = log.validated;
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public async Task<IActionResult> Error()
        {
            var log = await LoggedIn();
            ViewData["LoggedIn"] = log.validated;
            return View();
        }
    }
}
