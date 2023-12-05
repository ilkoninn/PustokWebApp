using Newtonsoft.Json;

namespace WebAppRelation.ViewComponents
{
    public class BasketViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;

        public BasketViewComponent(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
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
    }
}
