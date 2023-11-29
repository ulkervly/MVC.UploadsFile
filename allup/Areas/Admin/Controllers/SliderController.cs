using ALLUP2.DAL;
using ALLUP2.Models;
using Microsoft.AspNetCore.Mvc;

namespace ALLUP2.Areas.admin.Controllers
{
    [Area ("Admin")]
    public class SliderController : Controller
    {
        private readonly AppDbContext _context;
        public SliderController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            List<Slider> Sliders = _context.Sliders.ToList();
            return View(Sliders);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Slider slider)
        {
            if(!ModelState.IsValid) return View(slider);
            slider.Image = slider.ImageFile.FileName;
            _context.Sliders.Add(slider);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }


    }
}
