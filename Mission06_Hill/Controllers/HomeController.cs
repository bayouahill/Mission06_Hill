using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Mission06_Hill.Models;

namespace Mission06_Hill.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetToKnowJoel()
        {
            return View();
        }

        [HttpGet]
        public IActionResult MovieForm()
        {
            return View(new MovieEntryForm());
        }

    }
}
