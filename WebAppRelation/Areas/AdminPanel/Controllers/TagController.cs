using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAppRelation.Areas.AdminPanel.ViewModels;
using WebAppRelation.Models;

namespace WebAppRelation.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class TagController : Controller
    {
        AppDbContext _db;
        public TagController(AppDbContext db)
        {
            _db = db;
        }

        // <--- Table Section ---> 
        public async Task<IActionResult> Table()
        {
            AdminVM admin = new AdminVM();
            admin.Tags = await _db.Tags.ToListAsync();
            return View(admin);
        }


        // <--- Create Section --->
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateTagVM TagVM)
        {

            if (!ModelState.IsValid)
            {
                return View();
            }
            Tag newTag = new Tag
            {
                Name = TagVM.Name,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
            };

            _db.Tags.Add(newTag);
            await _db.SaveChangesAsync();

            return RedirectToAction("Table");
        }


        // <--- Update Section --->
        public async Task<IActionResult> Update(int Id)
        {
            ICollection<Tag> Tags = await _db.Tags.ToListAsync();
            Tag Tag = await _db.Tags.FindAsync(Id);
            CreateTagVM TagVM = new CreateTagVM
            {
                Id = Id,
                Name = Tag.Name,
            };

            return View(TagVM);
        }
        [HttpPost]
        public async Task<IActionResult> Update(CreateTagVM TagVM)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            Tag oldTag = await _db.Tags.FindAsync(TagVM.Id);
            oldTag.Name = TagVM.Name;
            oldTag.CreatedDate = oldTag.CreatedDate;
            oldTag.UpdatedDate = DateTime.Now;

            await _db.SaveChangesAsync();

            return RedirectToAction("Table");
        }
        public async Task<IActionResult> Delete(int Id)
        {

            Tag oldTag = await _db.Tags.FindAsync(Id);

            _db.Tags.Remove(oldTag);
            await _db.SaveChangesAsync();

            return RedirectToAction("Table");
        }

    }
}
