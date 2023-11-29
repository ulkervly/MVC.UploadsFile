using ALLUP2.DAL;
using ALLUP2.Models;
using ALLUP2.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ALLUP2.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            HomeVIewModel homevm = new HomeVIewModel()
            {
                Sliders = _context.Sliders.ToList(),

            };

            return View(homevm);
        }

        
    }
}