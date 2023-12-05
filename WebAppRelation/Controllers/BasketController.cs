using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace WebAppRelation.Controllers
{
    public class BasketController : Controller
    {
        private readonly AppDbContext _context;

        public BasketController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            List<BasketItemVM> basket = new();
            var books = _context.Books
                .Include(x => x.BookImages)
                .ToList();
            var cookieItems = Request.Cookies["Basket"];

            if(cookieItems != null)
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

                return View(basket);
            }
            else
            {
                return View(basket);
            }




        }

        public IActionResult AddItem(int id)
        {
            var cookiesJson = Request.Cookies["Basket"];

            List<CookieItemVM> cookies;

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
