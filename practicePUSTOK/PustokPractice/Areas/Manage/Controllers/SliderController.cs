using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PustokPractice.DAL;
using PustokPractice.Extensions;
using PustokPractice.Models;

namespace PustokPractice.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class SliderController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public SliderController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index()
        {
            List<Slider> sliders = _context.Sliders.ToList();

            return View(sliders);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Slider slider)
        {
            if (!ModelState.IsValid) return View(slider);
            if (slider.ImageFile != null)
            {
                if (slider.ImageFile.ContentType != "image/jpeg" && slider.ImageFile.ContentType != "image/png")
                {
                    ModelState.AddModelError("ImageFile", "File must be .png or .jpeg (.jpg)");
                    return View(slider);
                }
                if (slider.ImageFile.Length > 2097152)
                {
                    ModelState.AddModelError("ImageFile", "File size must be lower than 2mb!");
                    return View(slider);
                }

                slider.Image = Helper.SaveFile(_env.WebRootPath, "uploads/Sliders", slider.ImageFile);
            }
            else
            {
                ModelState.AddModelError("ImageFile", "Required!");
                return View();
            }
            _context.Sliders.Add(slider);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Update(int id)
        {
            Slider existSlider = _context.Sliders.Find(id);

            if (existSlider == null) return NotFound();

            return View(existSlider);
        }
        [HttpPost]
        public IActionResult Update(Slider slider)
        {
            Slider existSlider = _context.Sliders.FirstOrDefault(x => x.id== slider.id);
            if (existSlider == null) return NotFound();

            if (!ModelState.IsValid) return View();


            if (slider.ImageFile != null)
            {
                if (slider.ImageFile.ContentType != "image/jpeg" && slider.ImageFile.ContentType != "image/png")
                {
                    ModelState.AddModelError("ImageFile", "File must be .png or .jpeg (.jpg)");
                    return View(slider);
                }
                if (slider.ImageFile.Length > 2097152)
                {
                    ModelState.AddModelError("ImageFile", "File size must be lower than 2mb!");
                    return View(slider);
                }

                string deletePath = Path.Combine(_env.WebRootPath, "uploads/Sliders", existSlider.Image);

                if (System.IO.File.Exists(deletePath))
                {
                    System.IO.File.Delete(deletePath);
                }
                existSlider.Image = Helper.SaveFile(_env.WebRootPath, "uploads/Sliders", slider.ImageFile);
            }

            //existSlider.Image = slider.Image;
            existSlider.Title = slider.Title;
            existSlider.Description = slider.Description;
            existSlider.ButtonText = slider.ButtonText;
            existSlider.RedirectUrl = slider.RedirectUrl;

            _context.SaveChanges();

            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id)
        {
            Slider slider = _context.Sliders.FirstOrDefault(x => x.id == id);
            if (slider == null) return Json(new { status = 404 });

            _context.Sliders.Remove(slider);
            _context.SaveChanges();

            return Json(new { status = 200 });
        }
    }
}
