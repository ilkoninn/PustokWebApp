using Microsoft.AspNetCore.Mvc;
using WebAppRelation.Areas.AdminPanel.ViewModels;

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

        public IActionResult Table()
        {
            AdminVM admin = new AdminVM();
            admin.Categories = _db.Categories
                .ToList();
            return View(admin);
        }
        public IActionResult Create()
        {
            ICollection<Category> categories = _db.Categories.ToList();
            CreateCategoryVM categoryVM = new CreateCategoryVM()
            {
                categories = categories
            };
            return View(categoryVM);
        }
        [HttpPost]
        public IActionResult Create(CreateCategoryVM categoryVM)
        {

            if (!ModelState.IsValid)
            {
                var message = string.Join(" | ", ModelState.Values
            .SelectMany(v => v.Errors)
            .Select(e => e.ErrorMessage));
                Console.WriteLine(message);
                return View();
            }
            Category category = new Category();
            category.Name = categoryVM.Name;
            if(categoryVM.ParentCategoryId != "null")
            {
                category.ParentCategoryId = int.Parse(categoryVM.ParentCategoryId);
            }
            _db.Categories.Add(category);
            _db.SaveChanges();
            return RedirectToAction("Table");
        }
        public IActionResult Update(int Id)
        {
            return View();
        }
        [HttpPost]
        public IActionResult Update(Category Category)
        {
            return View();
        }
        public IActionResult Delete(Category Category)
        {
            return View();
        }

    }
}
