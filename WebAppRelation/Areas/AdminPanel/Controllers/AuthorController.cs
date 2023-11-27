using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAppRelation.Areas.AdminPanel.ViewModels;
using WebAppRelation.Models;

namespace WebAppRelation.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class AuthorController : Controller
    {
        AppDbContext _db;
        public AuthorController(AppDbContext db)
        {
            _db = db;
        }

        // <--- Table Section ---> 
        public async Task<IActionResult> Table()
        {
            AdminVM admin = new AdminVM();
            admin.Authors = await _db.Authors
                .ToListAsync();
            return View(admin);
        }


        // <--- Create Section --->
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateAuthorVM AuthorVM)
        {

            if (!ModelState.IsValid)
            {
                return View();
            }
            Author Author = new Author
            {
                Name = AuthorVM.Name,
                Surname = AuthorVM.Surname,
            };

            _db.Authors.Add(Author);
            await _db.SaveChangesAsync();

            return RedirectToAction("Table");
        }


        // <--- Update Section --->
        public async Task<IActionResult> Update(int Id)
        {
            ICollection<Author> Authors = await _db.Authors.ToListAsync();
            Author Author = await _db.Authors.FindAsync(Id);
            CreateAuthorVM AuthorVM = new CreateAuthorVM
            {
                Name = Author.Name,
                Surname = Author.Surname,
            };

            return View(AuthorVM);
        }
        [HttpPost]
        public async Task<IActionResult> Update(CreateAuthorVM AuthorVM)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            Author oldAuthor = await _db.Authors.FindAsync(AuthorVM.Id);
            oldAuthor.Name = AuthorVM.Name;
            oldAuthor.Surname = AuthorVM.Surname;

            await _db.SaveChangesAsync();

            return RedirectToAction("Table");
        }
        public async Task<IActionResult> Delete(int Id)
        {

            Author oldAuthor = await _db.Authors.FindAsync(Id);

            _db.Authors.Remove(oldAuthor);
            await _db.SaveChangesAsync();

            return RedirectToAction("Table");
        }

    }
}
