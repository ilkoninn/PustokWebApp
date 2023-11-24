
using Microsoft.AspNetCore.Mvc;
using WebAppRelation.Areas.AdminPanel.ViewModels;
using WebAppRelation.Models;

namespace WebAppRelation.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class BookImagesController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;
        public BookImagesController(AppDbContext db, IWebHostEnvironment environment)
        {
            _db = db;
            _env = environment;
        }

        public IActionResult Table()
        {
            AdminVM admin = new AdminVM();
            admin.BookImages = _db.BookImages
                .Include(x => x.Book)
                .ToList();
            return View(admin);
        }
        public IActionResult Create()
        {
            ViewData["Books"] = _db.Books.ToList();
            return View();
        }
        [HttpPost]
        public IActionResult Create(BookImages BookImage)
        {
            ViewData["Books"] = _db.Books.ToList();
            
            if (!BookImage.ImageFile.ContentType.Contains("image"))
            {
                ModelState.AddModelError("ImageFile", "You can upload only images");
                return View(BookImage);
            }
            if (BookImage.ImageFile.Length > 2097152)
            {
                ModelState.AddModelError("ImageFile", "The maximum size of image is 2MB!");
                return View(BookImage);
            }

            BookImage.ImgUrl = BookImage.ImageFile.Upload(_env.WebRootPath, @"\Upload\SliderImages\");

            if (!ModelState.IsValid)
            {
                return View(BookImage);
            }

            BookImage.Book = _db.Books.Find(BookImage.BookId);

            _db.BookImages.Add(BookImage);
            _db.SaveChanges();
            return RedirectToAction("Table");
        }
        public IActionResult Update(int Id)
        {
            return View();
        }
        [HttpPost]
        public IActionResult Update(BookImages BookImage)
        {
            return RedirectToAction("Table");
        }
        public IActionResult Delete(BookImages BookImage)
        {
            return RedirectToAction("Table");
        }

    }
}
