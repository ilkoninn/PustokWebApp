using Microsoft.AspNetCore.Mvc;
using WebAppRelation.Areas.AdminPanel.ViewModels;
using WebAppRelation.Models;

namespace WebAppRelation.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class BookController : Controller
    {
        AppDbContext _db;
        public BookController(AppDbContext db)
        {
            _db = db;
        }

        public IActionResult Table()
        {
            AdminVM admin = new AdminVM();
            admin.Books = _db.Books
                .Include(x => x.Category)
                .Include(x => x.Brand)
                .ToList();
            return View(admin);
        }
        [HttpGet]
        public IActionResult Create()
        {
            ViewData["Categories"] = _db.Categories.ToList();
            ViewData["Brands"] = _db.Brands.ToList();
            return View();
        }
        [HttpPost]
        public IActionResult Create(Book book)
        {
            ViewData["Categories"] = _db.Categories.ToList();
            ViewData["Brands"] = _db.Brands.ToList();
            if (!ModelState.IsValid)
            {
                return View(book);
            }

            book.Category = _db.Categories.FirstOrDefault(x => x.Id == book.CategoryId);
            book.Brand = _db.Brands.FirstOrDefault(x => x.Id == book.BrandId);

            _db.Books.Add(book);
            _db.SaveChanges();
            return RedirectToAction("Table");
        }
        [HttpGet]
        public IActionResult Update(int Id)
        {
            Book Book = _db.Books.Find(Id);
            ViewData["Categories"] = _db.Categories.ToList();
            ViewData["Brands"] = _db.Brands.ToList();

            return View(Book);
        }
        [HttpPost]
        public IActionResult Update(Book newBook)
        {
            Book oldBook = _db.Books.Find(newBook.Id);
            ViewData["Categories"] = _db.Categories.ToList();
            ViewData["Brands"] = _db.Brands.ToList();

            oldBook.Title = newBook.Title;
            oldBook.Description = newBook.Description;
            oldBook.BookCode = newBook.BookCode;
            oldBook.Price = newBook.Price;
            oldBook.Availability = newBook.Availability;
            oldBook.CreatedDate = newBook.CreatedDate;
            oldBook.Author = newBook.Author;
            oldBook.Category = _db.Categories.FirstOrDefault(x => x.Id == newBook.CategoryId);
            oldBook.Brand = _db.Brands.FirstOrDefault(x => x.Id == newBook.BrandId);

            _db.SaveChanges();

            return RedirectToAction("Table");
        }
        public IActionResult Delete(int Id)
        {
            Book oldBook = _db.Books.Find(Id);
            _db.Books.Remove(oldBook);
            _db.SaveChanges();

            return RedirectToAction("Table");
        }

    }
}
