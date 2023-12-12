using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace WebAppRelation.Controllers
{
    public class BasketController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;



        public BasketController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            List<BasketItemVM> basket = new();

            var books = _context.Books
                .Include(x => x.BookImages)
                .ToList();
            var cookieItems = Request.Cookies["Basket"];

            if (User.Identity.IsAuthenticated)
            {
                AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);

                List<BasketItem> basketItems = await _context.BasketItems
                    .Where(x => x.AppUserId == user.Id)
                    .Include(x => x.Book) 
                    .ThenInclude(x => x.BookImages)
                    .ToListAsync();

                foreach (var item in basketItems)
                {
                    basket.Add(new BasketItemVM()
                    {
                        Id = item.Id,
                        Title = item.Book.Title,
                        Price = item.Book.Price,
                        ImgUrl = item.Book.BookImages.FirstOrDefault().ImgUrl,
                        Count = item.Count,
                    });
                }

            }
            else
            {
                if (cookieItems != null)
                {
                    var cookies = JsonConvert.DeserializeObject<List<CookieItemVM>>(cookieItems);

                    foreach (var cookie in cookies)
                    {
                        foreach (var book in books)
                        {
                            if (cookie.Id == book.Id)
                            {
                                basket.Add(new BasketItemVM
                                {
                                    Id = book.Id,
                                    ImgUrl = book.BookImages.FirstOrDefault(x => x.IsPrime == true).ImgUrl,
                                    Price = book.Price,
                                    Title = book.Title,
                                    Count = cookie.Count,
                                });
                            }
                        }
                    }
                }
            }

            return View(basket);
        }

        public async Task<IActionResult> AddItem(int id)
        {
            var cookiesJson = Request.Cookies["Basket"];

            List<CookieItemVM> cookies;

            if (User.Identity.IsAuthenticated)
            {
                AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
                Book book = await _context.Books
                    .Include(x => x.BookImages)
                    .FirstOrDefaultAsync(x => x.Id == id);

                BasketItem oldBasketItem = await _context.BasketItems.FirstOrDefaultAsync(x => x.BookId == id && x.AppUserId == user.Id);

                if(oldBasketItem == null)
                {
                    BasketItem newBasketItem = new BasketItem()
                    {
                        Price = book.Price,
                        Count = 1,
                        AppUser = user,
                        Book = book,
                    };
                    
                    await _context.BasketItems.AddAsync(newBasketItem);
                }
                else
                {
                    oldBasketItem.Count += 1;
                }

                await _context.SaveChangesAsync();
            }
            else
            {
                if (cookiesJson != null)
                {
                    cookies = JsonConvert.DeserializeObject<List<CookieItemVM>>(cookiesJson);

                    foreach (var cookie in cookies)
                    {
                        if (cookie.Id == id)
                        {
                            cookie.Count += 1;
                            Response.Cookies.Append("Basket", JsonConvert.SerializeObject(cookies));

                            return RedirectToAction("Home", "Home");
                        }
                    }

                    CookieItemVM CookieItemVM = new CookieItemVM()
                    {
                        Id = id,
                        Count = 1
                    };

                    cookies.Add(CookieItemVM);

                    Response.Cookies.Append("Basket", JsonConvert.SerializeObject(cookies));
                }
                else
                {
                    cookies = new List<CookieItemVM>();
                    CookieItemVM CookieItemVM = new CookieItemVM()
                    {
                        Id = id,
                        Count = 1
                    };
                    cookies.Add(CookieItemVM);
                    Response.Cookies.Append("Basket", JsonConvert.SerializeObject(cookies));
                }
            }

            return RedirectToAction("Home", "Home");
        }

        public ActionResult DeleteItem(int id)
        {
            var cookiesJson = Request.Cookies["Basket"];

            List<CookieItemVM> cookies;

            if (cookiesJson != null)
            {
                cookies = JsonConvert.DeserializeObject<List<CookieItemVM>>(cookiesJson);

                var cookie = cookies.FirstOrDefault(c => c.Id == id);
                if(cookie != null)
                {
                    cookies.Remove(cookie);
                }

                Response.Cookies.Append("Basket", JsonConvert.SerializeObject(cookies));
            }

            return RedirectToAction("Index");
        }
    }
}
