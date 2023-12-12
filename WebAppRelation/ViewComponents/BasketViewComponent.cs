using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace WebAppRelation.ViewComponents
{
    public class BasketViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public BasketViewComponent(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
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
            }


            return View(basket);
        }
    }
}
