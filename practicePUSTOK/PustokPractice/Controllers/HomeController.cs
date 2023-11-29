using Microsoft.AspNetCore.Mvc;
using PustokPractice.DAL;
using PustokPractice.Models;

namespace PustokPractice.Controllers
{
    public class HomeController:Controller
    {
       
        public IActionResult Index()
        {
           
            return View();
        }
    }
}
