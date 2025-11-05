using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace YourLocalShop.Controllers
{
    public class HomeController(ILogger<HomeController> logger) : Controller
    {
        private readonly ILogger<HomeController> _logger = logger;

        [HttpGet]
        public IActionResult Index() => View();

        [HttpGet]
        public IActionResult Account() => RedirectToAction("Account", "Account");
    }
}