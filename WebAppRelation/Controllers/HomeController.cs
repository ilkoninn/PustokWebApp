    

namespace WebAppRelation.Controllers;

public class HomeController : Controller
{
    AppDbContext _context;
    public HomeController(AppDbContext context)
    {
        _context = context;
    }
    public IActionResult Home()
    {
        HomeVM homeVM = new HomeVM();
        homeVM.blogs = _context.Blogs
            .ToList();
        homeVM.books = _context.Books
            .Include(x => x.BookImages)
            .ToList();

        return View(homeVM);
    }
}