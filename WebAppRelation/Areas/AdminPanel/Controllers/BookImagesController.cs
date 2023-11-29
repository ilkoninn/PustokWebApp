
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
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

        // <--- Table Section --->
        public async Task<IActionResult> Table()
        {
            AdminVM admin = new AdminVM();
            admin.BookImages = await _db.BookImages.ToListAsync();
            admin.Books = await _db.Books.ToListAsync();

            return View(admin);
        }


        // <--- Create Section --->
        public async Task<IActionResult> Create()
        {
            ICollection<Book> books = await _db.Books.ToListAsync();
            CreateBookImageVM createBookImageVM = new CreateBookImageVM
            {
                Books = books,
            };
            return View(createBookImageVM);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateBookImageVM createBookImageVM)
        {
            if(createBookImageVM.BookId == "null")
            {
                createBookImageVM.Books = await _db.Books.ToListAsync();
                return View(createBookImageVM);
            }
            if (!createBookImageVM.ImageFile.ContentType.Contains("image"))
            {
                ModelState.AddModelError("ImageFile", "You can upload only images");
                return View(createBookImageVM);
            }
            if (createBookImageVM.ImageFile.Length > 2097152)
            {
                ModelState.AddModelError("ImageFile", "The maximum size of image is 2MB!");
                return View(createBookImageVM);
            }


            if (!ModelState.IsValid)
            {
                return View(createBookImageVM);
            }

            createBookImageVM.ImgUrl = createBookImageVM.ImageFile.Upload(_env.WebRootPath, @"\Upload\BookImages\");
            BookImages bookImages = new BookImages
            {
                ImgUrl = createBookImageVM.ImgUrl,
                BookId = int.Parse(createBookImageVM.BookId),
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
            };

            _db.BookImages.Add(bookImages);
            await _db.SaveChangesAsync();
            return RedirectToAction("Table");
        }


        // <--- Update Section --->
        public async Task<IActionResult> Update(int Id)
        {
            BookImages oldBookImages = await _db.BookImages.FirstOrDefaultAsync(x => x.Id == Id);
            CreateBookImageVM createBookImageVM = new CreateBookImageVM
            {
                ImgUrl = oldBookImages.ImgUrl,
                BookId = $"{oldBookImages.BookId}",
                Books = await _db.Books.ToListAsync(),
            };
            return View(createBookImageVM);
        }
        [HttpPost]
        public async Task<IActionResult> Update(CreateBookImageVM createBookImageVM)
        {
            BookImages oldBookImages = await _db.BookImages.FindAsync(createBookImageVM.Id);
            if (createBookImageVM.BookId == "null")
            {
                createBookImageVM.Books = await _db.Books.ToListAsync();
                return View(createBookImageVM);
            }

            if (!createBookImageVM.ImageFile.ContentType.Contains("image"))
            {
                ModelState.AddModelError("ImageFile", "You can upload only images");
                return View(createBookImageVM);
            }
            if (createBookImageVM.ImageFile.Length > 2097152)
            {
                ModelState.AddModelError("ImageFile", "The maximum size of image is 2MB!");
                return View(createBookImageVM);
            }


            if (!ModelState.IsValid)
            {
                return View(createBookImageVM);
            }

            createBookImageVM.ImgUrl = createBookImageVM.ImageFile.Upload(_env.WebRootPath, @"\Upload\BookImages\");
            oldBookImages.ImgUrl = createBookImageVM.ImgUrl;
            oldBookImages.BookId = int.Parse(createBookImageVM.BookId);
            oldBookImages.CreatedDate = oldBookImages.CreatedDate;
            oldBookImages.UpdatedDate = DateTime.Now;

            _db.BookImages.Add(oldBookImages);
            await _db.SaveChangesAsync();
            return RedirectToAction("Table");
        }
        public async Task<IActionResult> Delete(CreateBookImageVM createBookImageVM)
        {
            BookImages BookImage = await _db.BookImages.FindAsync(createBookImageVM.Id);

            _db.BookImages.Remove(BookImage);
            FileManager.Delete(BookImage.ImgUrl, _env.WebRootPath, @"\Upload\BookImages\");

            await _db.SaveChangesAsync();

            return RedirectToAction("Table");
        }

    }
}
