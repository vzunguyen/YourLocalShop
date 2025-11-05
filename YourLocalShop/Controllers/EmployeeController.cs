using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace YourLocalShop.Controllers
{
    [Authorize(Roles = "Employee")]
    public class EmployeeController : Controller
    {
        [HttpGet]
        public IActionResult Dashboard() => View();
    }
}