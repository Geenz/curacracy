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
        [Route("/")]
        public IActionResult Index()
        {
            var log = LoggedIn().Result;
            ViewData["LoggedIn"] = log.validated;
            return View();
        }

        [Route("/about")]
        public async Task<IActionResult> About()
        {
            var log = await LoggedIn();
            ViewData["LoggedIn"] = log.validated;
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        [Route("/contact")]
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
