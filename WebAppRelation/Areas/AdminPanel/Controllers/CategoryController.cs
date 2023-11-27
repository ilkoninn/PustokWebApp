using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAppRelation.Areas.AdminPanel.ViewModels;
using WebAppRelation.Models;

namespace WebAppRelation.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class CategoryController : Controller
    {
        AppDbContext _db;
        public CategoryController(AppDbContext db)
        {
            _db = db;
        }

        // <--- Table Section ---> 
        public async Task<IActionResult> Table()
        {
            AdminVM admin = new AdminVM();
            admin.Categories = await _db.Categories
                .ToListAsync();
            return View(admin);
        }


        // <--- Create Section --->
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ICollection<Category> categories = await _db.Categories.ToListAsync();
            CreateCategoryVM categoryVM = new CreateCategoryVM()
            {
                categories = categories
            };
            return View(categoryVM);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateCategoryVM categoryVM)
        {

            if (!ModelState.IsValid)
            {
                return View();
            }
            Category category = new Category();
            category.Name = categoryVM.Name;
            if (categoryVM.ParentCategoryId != "null")
            {
                category.ParentCategoryId = int.Parse(categoryVM.ParentCategoryId);
            }

            _db.Categories.Add(category);
            await _db.SaveChangesAsync();

            return RedirectToAction("Table");
        }


        // <--- Update Section --->
        public async Task<IActionResult> Update(int Id)
        {
            ICollection<Category> categories = await _db.Categories.ToListAsync();
            Category category = await _db.Categories.FindAsync(Id);
            CreateCategoryVM categoryVM = new CreateCategoryVM
            {
                Id = Id,
                Name = category.Name,
                ParentCategoryId = $"{category.ParentCategoryId}",
                categories = categories
            };

            return View(categoryVM);
        }
        [HttpPost]
        public async Task<IActionResult> Update(CreateCategoryVM categoryVM)
        {
            Category oldCategory = new Category();

            if (!ModelState.IsValid)
            {
                return View();
            }

            oldCategory.Name = categoryVM.Name;
            if (categoryVM.ParentCategoryId != "null")
            {
                oldCategory.ParentCategoryId = int.Parse(categoryVM.ParentCategoryId);
            }

            Category newCategory = await _db.Categories.FindAsync(categoryVM.Id);
            newCategory.Name = oldCategory.Name;
            newCategory.ParentCategoryId = oldCategory.ParentCategoryId;
            await _db.SaveChangesAsync();

            return RedirectToAction("Table");
        }
        public async Task<IActionResult> Delete(int Id)
        {
            
            Category oldCategory = await _db.Categories.FindAsync(Id);
            foreach (var item in await _db.Categories.ToListAsync())
            {
                if(item.ParentCategoryId == oldCategory.Id)
                {
                    _db.Categories.Remove(item);
                }
            }

            _db.Categories.Remove(oldCategory);
            await _db.SaveChangesAsync();

            return RedirectToAction("Table");
        }

    }
}
