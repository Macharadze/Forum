using Forum.Domain.Enums;
using Forum.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Forum.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public ActionResult Error(string messageJson = "")
        {
    
            var errorMesage = JsonConvert.DeserializeObject<ErrorViewModel>(messageJson);

            return View(errorMesage);
        }
        
    }
}