using Microsoft.AspNetCore.Mvc;
using WebAppRelation.Areas.AdminPanel.ViewModels;

namespace WebAppRelation.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class BrandController : Controller
    {
        AppDbContext _db;
        public BrandController(AppDbContext db)
        {
            _db = db;
        }

        public IActionResult Table()
        {
            AdminVM admin = new AdminVM();
            admin.Brands = _db.Brands
                .ToList();
            return View(admin);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Brand Brand)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            _db.Brands.Add(Brand);
            _db.SaveChanges();

            return RedirectToAction("Table");
        }
        public IActionResult Update(int Id)
        {
            return View();
        }
        [HttpPost]
        public IActionResult Update(Brand Brand)
        {
            return View();
        }
        public IActionResult Delete(Brand Brand)
        {
            return View();
        }

    }
}
