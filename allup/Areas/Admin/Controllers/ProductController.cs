using ALLUP2.DAL;
using ALLUP2.Extensions;
using ALLUP2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ALLUP2.Areas.Admin.Controllers
{
    [Area("admin")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public ProductController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index()
        {
            List<Product> products = _context.Products.ToList();
            return View(products);
        }
        public IActionResult Create()
        {

            ViewBag.Tags = _context.Tags.ToList();
            return View();
        }
        [HttpPost]
        public IActionResult Create(Product Product)
        {

            ViewBag.Tags = _context.Tags.ToList();
            if (!ModelState.IsValid)
            {

                return View();
            }

            bool check = true;
            if (Product.TagIds != null)
            {
                foreach (var item in Product.TagIds)
                {
                    if (!_context.Tags.Any(x => x.Id == item))
                    {
                        check = false;
                        break;
                    }
                }
            }
            if (check)
            {
                if (Product.TagIds != null)
                {
                    foreach (var tagId in Product.TagIds)
                    {
                        ProductTag productTag = new ProductTag()
                        {
                            Product = Product,
                            TagId = tagId,
                        };
                        _context.ProductTags.Add(productTag);
                    }
                }

            }
            else
            {

                ModelState.AddModelError("TagId", "Tag id not found!");
                return View();
            }
            if (Product.ProductPosterImageFile != null)
            {
                if (Product.ProductPosterImageFile.ContentType != "image/jpeg" && Product.ProductPosterImageFile.ContentType != "image/png")
                {
                    ModelState.AddModelError("ProductPosterImageFile", "File must be .png or .jpeg (.jpg)");
                    return View();
                }
                if (Product.ProductPosterImageFile.Length > 2097152)
                {
                    ModelState.AddModelError("ProductPosterImageFile", "File size must be lower than 2mb!");
                    return View();
                }
            }
            ProductImage ProductImage = new ProductImage()
            {
                Product = Product,
                ImageUrl = Helper.SaveFile(_env.WebRootPath, "uploads/Products", Product.ProductPosterImageFile),
                IsPoster = true,
            };
            _context.ProductImages.Add(ProductImage);



            if (Product.ProductHoverImageFile != null)
            {
                if (Product.ProductHoverImageFile.ContentType != "image/jpeg" && Product.ProductHoverImageFile.ContentType != "image/png")
                {
                    ModelState.AddModelError("ProductHoverImageFile", "File must be .png or .jpeg (.jpg)");
                    return View();
                }
                if (Product.ProductPosterImageFile.Length > 2097152)
                {
                    ModelState.AddModelError("ProductHoverImageFile", "File size must be lower than 2mb!");
                    return View();
                }
            }
            ProductImage ProductImage1 = new ProductImage()
            {
                Product = Product,
                ImageUrl = Helper.SaveFile(_env.WebRootPath, "uploads/Products", Product.ProductHoverImageFile),
                IsPoster = false,
            };
            _context.ProductImages.Add(ProductImage1);

            if (Product.ImageFiles != null)
            {
                foreach (var item in Product.ImageFiles)
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
                    ProductImage bi = new ProductImage()
                    {
                        Product = Product,
                        ImageUrl = Helper.SaveFile(_env.WebRootPath, "uploads/Products", item),
                        IsPoster = null,
                    };
                    _context.ProductImages.Add(bi);
                }

                
            }
            _context.Products.Add(Product);
            _context.SaveChanges();

            return RedirectToAction("index");
        }
            public IActionResult Update(int id)
            {

                ViewBag.Tags = _context.Tags.ToList();

                Product existProduct = _context.Products.Include(x => x.ProductTags).FirstOrDefault(y => y.Id == id);
                if (existProduct == null) return NotFound();

                existProduct.TagIds = existProduct.ProductTags.Select(p => p.TagId).ToList();
                return View(existProduct);


            }
            [HttpPost]
            public IActionResult Update(Product Product)
            {

                ViewBag.Tags = _context.Tags.ToList();

                if (!ModelState.IsValid)
                {
                    return View();
                }
                Product existProduct = _context.Products.Include(x => x.ProductTags).FirstOrDefault(y => y.Id == Product.Id);
                if (existProduct == null) return NotFound();


                existProduct.ProductTags.RemoveAll(x => !Product.TagIds.Any(y => y == x.TagId));

                foreach (var item in Product.TagIds.Where(x => !existProduct.ProductTags.Any(y => y.TagId == x)))
                {
                    ProductTag ProductTag = new ProductTag()
                    {
                        TagId = item,
                    };
                    _context.ProductTags.Add(ProductTag);
                }

                existProduct.Name = Product.Name;
                existProduct.Description = Product.Description;
                existProduct.Costprice = Product.Costprice;
                existProduct.Saleprice = Product.Saleprice;
                existProduct.DiscountPercent = Product.DiscountPercent;


                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            public IActionResult Delete(int id)
            {
                ViewBag.Tags = _context.Tags.ToList();

                if (id == null) return NotFound();

                Product product = _context.Products.FirstOrDefault(p => p.Id == id);

                if (product == null) return NotFound();


                return View(product);
            }

            [HttpPost]
            public IActionResult Delete(Product product)
            {
                ViewBag.Tags = _context.Tags.ToList();

                Product wantedProduct = _context.Products.FirstOrDefault(p => p.Id == product.Id);

                if (wantedProduct == null)
                {
                    return NotFound();
                }

                _context.Products.Remove(wantedProduct);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }

        
    }
}
