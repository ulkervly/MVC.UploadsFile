using ALLUP2.DAL;
using ALLUP2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ALLUP2.Areas.Admin.Controllers
{
    [Area("admin")]
    public class TagController : Controller
    {
        private readonly AppDbContext _context;

        public TagController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            List<Tag> tags = _context.Tags.ToList();
            return View(tags);
        }
        public IActionResult Create()
        {

           
            return View();
        }
        [HttpPost]
        public IActionResult Create(Tag tag)
        {

            if (_context.Tags.Any(t => t.Name.ToLower() == tag.Name.ToLower()))
            {
                ModelState.AddModelError("Name", "Tag has created");
                return View();
            }

            if (!ModelState.IsValid)
            {

                return View();
            }

   

            _context.Tags.Add(tag);
            _context.SaveChanges();

            return RedirectToAction("index");
        }
        public IActionResult Update(int id)
        {

            if (id == null) return NotFound();

            Tag tag = _context.Tags.FirstOrDefault(t => t.Id == id);

            if (tag == null) return NotFound();

            return View(tag);

        }
        [HttpPost]
        public IActionResult Update(Tag tag)
        {

         

            if (!ModelState.IsValid)
            {
                return View();
            }
            Tag existTag = _context.Tags.FirstOrDefault(t => t.Id == tag.Id);
            if (existTag == null) return NotFound();

            if (_context.Tags.Any(t => t.Id != tag.Id && t.Name.ToLower() == tag.Name.ToLower()))
            {
                ModelState.AddModelError("Name", "Tag has  created!");
                return View();
            }
            existTag.Name = tag.Name;

            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id)
        {
            if (id == null) return NotFound();

            Tag tag = _context.Tags.FirstOrDefault(t => t.Id == id);
            return View(tag);
        }

        [HttpPost]
        public IActionResult Delete(Tag tag)
        {

            Tag existTag = _context.Tags.FirstOrDefault(t => t.Id == tag.Id);

            if (existTag == null)
            {
                return NotFound();
            }

            _context.Tags.Remove(existTag);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }


    }
}

