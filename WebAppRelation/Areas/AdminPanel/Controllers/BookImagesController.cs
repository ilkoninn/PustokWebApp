using Microsoft.AspNetCore.Mvc;
using WebAppRelation.Areas.AdminPanel.ViewModels;

namespace WebAppRelation.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class BookImagesController : Controller
    {
        AppDbContext _db;
        public BookImagesController(AppDbContext db)
        {
            _db = db;
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
            return View();
        }
        [HttpPost]
        public IActionResult Create(BookImages BookImage)
        {
            return View();
        }
        public IActionResult Update(int Id)
        {
            return View();
        }
        [HttpPost]
        public IActionResult Update(BookImages BookImage)
        {
            return View();
        }
        public IActionResult Delete(BookImages BookImage)
        {
            return View();
        }

    }
}
