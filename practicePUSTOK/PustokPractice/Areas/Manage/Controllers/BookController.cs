using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PustokPractice.DAL;
using PustokPractice.Extensions;
using PustokPractice.Models;
using System.Linq;

namespace PustokPractice.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class BookController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public BookController(AppDbContext context,IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index()
        {
            var books=_context.Books.ToList();
           
            return View(books);
        }
        public IActionResult Create()
        {
            ViewBag.Authors = _context.Authors.ToList();
            ViewBag.Genre=_context.Genre.ToList();
            ViewBag.Tags = _context.Tags.ToList();
            return View();
        }
        [HttpPost]
        public IActionResult Create(Book book)
        {
            ViewBag.Authors = _context.Authors.ToList();
            ViewBag.Genre = _context.Genre.ToList();
            ViewBag.Tags = _context.Tags.ToList();
            if (!ModelState.IsValid)
            {
             
                return View();
            }
            if (!_context.Genre.Any(a => a.id == book.GenreId))
            {
                ModelState.AddModelError("GenreId", "genre is not found!");
                return View();
            }
            
            if (!_context.Authors.Any(a => a.id == book.AuthorId))
            {
                ModelState.AddModelError("AuthorId", "Author is not found!");
                return View();
            
            }
            bool check = true;
            if (book.TagIds != null)
            {
                foreach (var item in book.TagIds)
                {
                    if (!_context.Tags.Any(x => x.id == item))
                    {
                        check = false;
                        break;
                    }
                }
            }
            if (check)
            {
                foreach (var item in book.TagIds)
                {
                    BookTag bookTag = new BookTag()
                    {
                        Book = book,
                        TagId = item,
                    };
                    _context.BookTags.Add(bookTag);
                }
            }
            else
            {
                ModelState.AddModelError("TagId", "Error");
                return View();
            }
            if (book.BookPosterImageFile !=null)
            {
                if (book.BookPosterImageFile.ContentType != "image/jpeg" && book.BookPosterImageFile.ContentType != "image/png")
                {
                    ModelState.AddModelError("BookPosterImageFile", "File must be .png or .jpeg (.jpg)");
                    return View();
                }
                if (book.BookPosterImageFile.Length > 2097152)
                {
                    ModelState.AddModelError("BookPosterImageFile", "File size must be lower than 2mb!");
                    return View();
                }
            }
            BookImage bookImage = new BookImage()
            {
                Book = book,
                ImageUrl = Helper.SaveFile(_env.WebRootPath, "uploads/Books", book.BookPosterImageFile),
                IsPoster = true,
             };
            _context.BookImages.Add(bookImage);



            if (book.BookHoverImageFile != null)
            {
                if (book.BookHoverImageFile.ContentType != "image/jpeg" && book.BookHoverImageFile.ContentType != "image/png")
                {
                    ModelState.AddModelError("BookHoverImageFile", "File must be .png or .jpeg (.jpg)");
                    return View();
                }
                if (book.BookPosterImageFile.Length > 2097152)
                {
                    ModelState.AddModelError("BookHoverImageFile", "File size must be lower than 2mb!");
                    return View();
                }
            }
            BookImage bookImage1 = new BookImage()
            {
                Book = book,
                ImageUrl = Helper.SaveFile(_env.WebRootPath, "uploads/Books", book.BookHoverImageFile),
                IsPoster = false,
            };
            _context.BookImages.Add(bookImage1);

            if (book.ImageFiles !=null)
            {
                foreach (var item in book.ImageFiles)
                {
                    if (item.ContentType != "image/jpeg" && item.ContentType != "image/png")
                    {
                        ModelState.AddModelError("ImageFiles", "File must be .png or .jpeg (.jpg)");
                        return View();
                    }
                    if (item.Length > 2097152)
                    {
                        ModelState.AddModelError("ImageFiles", "File size must be lower than 2mb!");
                        return View();
                    }
                    BookImage bi = new BookImage()
                    {
                        Book = book,
                        ImageUrl = Helper.SaveFile(_env.WebRootPath, "uploads/Books", item),
                        IsPoster = null,
                    };
                    _context.BookImages.Add(bi);
                }
            }





            _context.Books.Add(book);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
        public IActionResult Update(int id)
        {
            ViewBag.Authors = _context.Authors.ToList();
            ViewBag.Genre = _context.Genre.ToList();
            ViewBag.Tags = _context.Tags.ToList();

            Book existBook = _context.Books.Include(x => x.BookTags).Include(x=>x.BookImages).FirstOrDefault(x => x.id == id);
            if (existBook == null) return NotFound();

            existBook.TagIds = existBook.BookTags.Select(x => x.TagId).ToList();
            return View(existBook);


        }
        [HttpPost]
        public IActionResult Update(Book book)
        {
            ViewBag.Authors = _context.Authors.ToList();
            ViewBag.Genre = _context.Genre.ToList();
            ViewBag.Tags = _context.Tags.ToList();

            if (!ModelState.IsValid)
            {
                return View();
            }
            Book existBook = _context.Books.Include(x => x.BookTags).Include(x=>x.BookImages).FirstOrDefault(x => x.id == book.id);
            if (existBook == null) return NotFound();


            existBook.BookTags.RemoveAll(x => !book.TagIds.Any(y => y == x.TagId));

            foreach (var item in book.TagIds.Where(x => !existBook.BookTags.Any(y => y.TagId == x)))
            {
                BookTag bookTag = new BookTag()
                {
                    TagId = item,
                };
                _context.BookTags.Add(bookTag);
            }

            if (book.BookPosterImageFile != null)
            {
                if (book.BookPosterImageFile.ContentType != "image/jpeg" && book.BookPosterImageFile.ContentType != "image/png")
                {
                    ModelState.AddModelError("BookPosterImageFile", "File must be .png or .jpeg (.jpg)");
                    return View();
                }
                if (book.BookPosterImageFile.Length > 2097152)
                {
                    ModelState.AddModelError("BookPosterImageFile", "File size must be lower than 2mb!");
                    return View();
                }
            }
            BookImage bookImage = new BookImage()
            {
                Book = book,
                ImageUrl = Helper.SaveFile(_env.WebRootPath, "uploads/Books", book.BookPosterImageFile),
                IsPoster = true,
            };
            existBook.BookImages.Add(bookImage);



            if (book.BookHoverImageFile != null)
            {
                if (book.BookHoverImageFile.ContentType != "image/jpeg" && book.BookHoverImageFile.ContentType != "image/png")
                {
                    ModelState.AddModelError("BookHoverImageFile", "File must be .png or .jpeg (.jpg)");
                    return View();
                }
                if (book.BookPosterImageFile.Length > 2097152)
                {
                    ModelState.AddModelError("BookHoverImageFile", "File size must be lower than 2mb!");
                    return View();
                }
            }
            BookImage bookImage1 = new BookImage()
            {
                Book = book,
                ImageUrl = Helper.SaveFile(_env.WebRootPath, "uploads/Books", book.BookHoverImageFile),
                IsPoster = false,
            };
            existBook.BookImages.Add(bookImage1);

            existBook.BookImages.RemoveAll(bimg=> !book.BookImageIds.Contains(bimg.id) && bimg.IsPoster==null);
            if (book.ImageFiles != null)
            {
                foreach (var item in book.ImageFiles)
                {
                    if (item.ContentType != "image/jpeg" && item.ContentType != "image/png")
                    {
                        ModelState.AddModelError("ImageFiles", "File must be .png or .jpeg (.jpg)");
                        return View();
                    }
                    if (item.Length > 2097152)
                    {
                        ModelState.AddModelError("ImageFiles", "File size must be lower than 2mb!");
                        return View();
                    }
                    BookImage bi = new BookImage()
                    {
                        Book = book,
                        ImageUrl = Helper.SaveFile(_env.WebRootPath, "uploads/Books", item),
                        IsPoster = null,
                    };
                    //_context.BookImages.Add(bi);
                    existBook.BookImages.Add(bi);
                }
            }

            existBook.Name = book.Name;
            existBook.Description = book.Description;
            existBook.Costprice = book.Costprice;
            existBook.Saleprice = book.Saleprice;
            existBook.DiscountPercent = book.DiscountPercent;
            existBook.Code = book.Code;
            existBook.IsAvailable = book.IsAvailable;
            existBook.Tax = book.Tax;
            existBook.GenreId = book.GenreId;
            existBook.AuthorId = book.AuthorId;

            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id)
        {
           
            ViewBag.Tags = _context.Tags.ToList();

            if (id == null) return NotFound();

            Book book = _context.Books.FirstOrDefault(x => x.id == id);

            if (book == null) return NotFound();


            return View(book);
        }

        [HttpPost]
        public IActionResult Delete(Book book)
        {
            ViewBag.Tags = _context.Tags.ToList();

            Book wantedBook = _context.Books.FirstOrDefault(x => x.id == book.id);

            if (wantedBook == null)
            {
                return NotFound();
            }

            _context.Books.Remove(wantedBook);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
