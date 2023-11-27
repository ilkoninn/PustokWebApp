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

        public async Task<IActionResult> Table()
        {
            AdminVM admin = new AdminVM();
            admin.Brands = await _db.Brands.ToListAsync(); 
            admin.Authors = await _db.Authors.ToListAsync(); 
            admin.Categories = await _db.Categories.ToListAsync();
            admin.Books = await _db.Books.ToListAsync();

            return View(admin);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ICollection<Category> categories = await _db.Categories.ToListAsync();
            ICollection<Brand> brands = await _db.Brands.ToListAsync();
            ICollection<Author> authors = await _db.Authors.ToListAsync();

            CreateBookVM createBookVM = new CreateBookVM()
            {
                Categories = categories,
                Brands = brands,
                Authors = authors,
            };
            return View(createBookVM);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateBookVM createBookVM)
        {
            if(createBookVM.CategoryId == null || createBookVM.BrandId == null || createBookVM.AuthorId == null)
            {
                return View(createBookVM);
            }
            if (!ModelState.IsValid)
            {
                return View(createBookVM);
            }
            Book newBook = new Book
            {
                Title = createBookVM.Title,
                Description = createBookVM.Description,
                BookCode = createBookVM.BookCode,
                Price = createBookVM.Price,
                Availability = createBookVM.Availability,
                AuthorId = createBookVM.AuthorId,
                CategoryId = createBookVM.CategoryId,
                BrandId = createBookVM.BrandId,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
            };

            _db.Books.Add(newBook);
            await _db.SaveChangesAsync();
            return RedirectToAction("Table");
        }
        [HttpGet]
        public async Task<IActionResult> Update(int Id)
        {
            Book Book = _db.Books.Find(Id);
            CreateBookVM createBookVM = new CreateBookVM
            {
                Title = Book.Title,
                Description = Book.Description,
                BookCode = Book.BookCode,
                Price = Book.Price,
                CategoryId = Book.CategoryId,
                BrandId = Book.BrandId,
                AuthorId = Book.AuthorId,
                Availability = Book.Availability,
                Categories = await _db.Categories.ToListAsync(),
                Brands = await _db.Brands.ToListAsync(),
                Authors = await _db.Authors.ToListAsync(),
            };

            return View(createBookVM);
        }
        [HttpPost]
        public async Task<IActionResult> Update(CreateBookVM createBookVM)
        {
            Book oldBook = await _db.Books.FindAsync(createBookVM.Id);

            oldBook.Title = createBookVM.Title;
            oldBook.Description = createBookVM.Description;
            oldBook.BookCode = createBookVM.BookCode;
            oldBook.Price = createBookVM.Price;
            oldBook.Availability = createBookVM.Availability;
            oldBook.AuthorId = createBookVM.AuthorId;
            oldBook.CategoryId = createBookVM.CategoryId; 
            oldBook.BrandId = createBookVM.BrandId;
            oldBook.CreatedDate = oldBook.CreatedDate;
            oldBook.UpdatedDate = DateTime.Now;


            await _db.SaveChangesAsync();

            return RedirectToAction("Table");
        }
        public async Task<IActionResult> Delete(int Id)
        {
            Book oldBook = await _db.Books.FindAsync(Id);
            _db.Books.Remove(oldBook);
            await _db.SaveChangesAsync();
            return RedirectToAction("Table");
        }

    }
}
