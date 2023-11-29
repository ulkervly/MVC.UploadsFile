using Microsoft.AspNetCore.Mvc;

namespace PustokPractice.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
