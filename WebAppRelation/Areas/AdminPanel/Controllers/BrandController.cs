using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAppRelation.Areas.AdminPanel.ViewModels;
using WebAppRelation.Models;

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

        // <--- Table Section ---> 
        public async Task<IActionResult> Table()
        {
            AdminVM admin = new AdminVM();
            admin.Brands = await _db.Brands.ToListAsync();
            return View(admin);
        }


        // <--- Create Section --->
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateBrandVM BrandVM)
        {

            if (!ModelState.IsValid)
            {
                return View();
            }
            Brand newBrand = new Brand 
            {
                Name = BrandVM.Name,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
            };

            _db.Brands.Add(newBrand);
            await _db.SaveChangesAsync();

            return RedirectToAction("Table");
        }


        // <--- Update Section --->
        public async Task<IActionResult> Update(int Id)
        {
            ICollection<Brand> Brands = await _db.Brands.ToListAsync();
            Brand Brand = await _db.Brands.FindAsync(Id);
            CreateBrandVM BrandVM = new CreateBrandVM
            {
                Id = Id,
                Name = Brand.Name,
            };

            return View(BrandVM);
        }
        [HttpPost]
        public async Task<IActionResult> Update(CreateBrandVM BrandVM)
        {if (!ModelState.IsValid)
            {
                return View();
            }

            Brand oldBrand = await _db.Brands.FindAsync(BrandVM.Id);
            oldBrand.Name = BrandVM.Name;
            oldBrand.CreatedDate = oldBrand.CreatedDate;
            oldBrand.UpdatedDate = DateTime.Now;

            await _db.SaveChangesAsync();

            return RedirectToAction("Table");
        }
        public async Task<IActionResult> Delete(int Id)
        {

            Brand oldBrand = await _db.Brands.FindAsync(Id);
           
            _db.Brands.Remove(oldBrand);
            await _db.SaveChangesAsync();

            return RedirectToAction("Table");
        }

    }
}
