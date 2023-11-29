using Microsoft.AspNetCore.Mvc;
using PustokPractice.DAL;
using PustokPractice.Models;

namespace PustokPractice.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class GenreController : Controller
    {
        private readonly AppDbContext _context;

        public GenreController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var genres=_context.Genre.ToList();
            return View(genres);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Genre genre)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (_context.Genre.Any(x => x.Name.ToLower() == genre.Name.ToLower()))
            {
                ModelState.AddModelError("Name", "genre already exist");
                return View();

                    
            }
            _context.Genre.Add(genre);
            _context.SaveChanges();
          
            return RedirectToAction("index");
        }


        public IActionResult Update(int id)
        {
            var existGenre= _context.Genre.FirstOrDefault(x=>x.id==id);
            if (existGenre==null) return NotFound();
            return View(existGenre);
        }
        [HttpPost]
        public IActionResult Update(Genre genre)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var existGenre = _context.Genre.FirstOrDefault(x => x.id ==genre.id);
            if(existGenre==null) return NotFound();
            if (_context.Genre.Any(x =>x.id!=genre.id  && x.Name.ToLower() == genre.Name.ToLower())) ;
            {
                ModelState.AddModelError("Name", "genre already exist");
                
            }
            return RedirectToAction("index");
            
            
           

        }
    }
}
