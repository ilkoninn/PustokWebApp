
namespace WebAppRelation.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class BookController : Controller
    {
        AppDbContext _db;
        public BookController(AppDbContext db)
        {
            _db = db;
        }


        // <--- Table Section --->
        public async Task<IActionResult> Table()
        {
            AdminVM admin = new AdminVM();
            admin.Brands = await _db.Brands.ToListAsync(); 
            admin.Authors = await _db.Authors.ToListAsync(); 
            admin.Categories = await _db.Categories.ToListAsync();
            admin.Books = await _db.Books
                .Include(x => x.Tags)
                .ThenInclude(x => x.Tag)
                .ToListAsync();
            admin.Tags = await _db.Tags.ToListAsync();

            return View(admin);
        }

        // <--- Create Section --->
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ICollection<Category> categories = await _db.Categories.ToListAsync();
            ICollection<Brand> brands = await _db.Brands.ToListAsync();
            ICollection<Author> authors = await _db.Authors.ToListAsync();
            ICollection<Tag> tags = await _db.Tags.ToListAsync();

            CreateBookVM createBookVM = new CreateBookVM()
            {
                Categories = categories,
                Brands = brands,
                Authors = authors,
                Tags = tags,
            };
            return View(createBookVM);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateBookVM createBookVM)
        {
            if(createBookVM.CategoryId == null || createBookVM.BrandId == null || createBookVM.AuthorId == null)
            {
                return View(createBookVM);
            }
            if (!ModelState.IsValid)
            {
                return View(createBookVM);
            }
            //List<BookTag> tags = new List<BookTag>();

            //foreach (int id in createBookVM.TagIds)
            //{
            //    if (await _db.Tags.AnyAsync(t => t.Id == id))
            //    {
            //        BookTag item = await _db.BookTags.Where(t => t.Id == id).FirstOrDefaultAsync();
            //        tags.Add(item);
            //    }
            //}

            Book newBook = new Book
            {
                Title = createBookVM.Title,
                Description = createBookVM.Description,
                BookCode = createBookVM.BookCode,
                Price = createBookVM.Price,
                Availability = createBookVM.Availability,
                AuthorId = createBookVM.AuthorId,
                CategoryId = createBookVM.CategoryId,
                BrandId = createBookVM.BrandId,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
            };

            //await _db.SaveChangesAsync();

            if (createBookVM.TagIds != null)
            {
                foreach (var tagId in createBookVM.TagIds)
                {
                    BookTag bookTag = new BookTag
                    {
                        Book = newBook,
                        TagId = tagId,
                    };

                    _db.BookTags.Add(bookTag);
                }
            }

            _db.Books.Add(newBook);
            await _db.SaveChangesAsync();
            return RedirectToAction("Table");
        }

        // <--- Update Section --->
        [HttpGet]
        public async Task<IActionResult> Update(int Id)
        {
            Book Book = await _db.Books
                .Include(x => x.Tags)
                .ThenInclude(x => x.Tag)
                .FirstOrDefaultAsync(p=> p.Id == Id);

            ICollection<Tag> tags = await _db.Tags.ToListAsync();

            List<int> tagIds = new();
            
            foreach (Tag tag in tags)
                foreach (var item in Book.Tags)
                    if(tag.Id == item.TagId)
                        tagIds.Add(tag.Id);

            CreateBookVM createBookVM = new CreateBookVM
            {
                Title = Book.Title,
                Description = Book.Description,
                BookCode = Book.BookCode,
                Price = Book.Price,
                CategoryId = Book.CategoryId,
                BrandId = Book.BrandId,
                AuthorId = Book.AuthorId,
                Availability = Book.Availability,
                Categories = await _db.Categories.ToListAsync(),
                Brands = await _db.Brands.ToListAsync(),
                Authors = await _db.Authors.ToListAsync(),
                Tags = tags,
                TagIds = tagIds,
            };

            return View(createBookVM);
        }
        [HttpPost]
        public async Task<IActionResult> Update(CreateBookVM createBookVM)
        {
            Book oldBook = await _db.Books
                .Include(x => x.Tags)
                .ThenInclude(x => x.Tag)
                .FirstOrDefaultAsync(x => x.Id == createBookVM.Id);

            oldBook.Title = createBookVM.Title;
            oldBook.Description = createBookVM.Description;
            oldBook.BookCode = createBookVM.BookCode;
            oldBook.Price = createBookVM.Price;
            oldBook.Availability = createBookVM.Availability;
            oldBook.AuthorId = createBookVM.AuthorId;
            oldBook.CategoryId = createBookVM.CategoryId; 
            oldBook.BrandId = createBookVM.BrandId;
            oldBook.CreatedDate = oldBook.CreatedDate;
            oldBook.UpdatedDate = DateTime.Now;

            oldBook.Tags.Clear();

            if (createBookVM.TagIds != null)
            {
                foreach (var tagId in createBookVM.TagIds)
                {
                    BookTag bookTag = new BookTag()
                    {
                        Book = oldBook,
                        TagId = tagId,
                    };

                    oldBook.Tags.Add(bookTag);
                }
            }

            await _db.SaveChangesAsync();

            return RedirectToAction("Table");
        }
        public async Task<IActionResult> Delete(int Id)
        {
            Book oldBook = await _db.Books.FindAsync(Id);
            _db.Books.Remove(oldBook);
            await _db.SaveChangesAsync();
            return RedirectToAction("Table");
        }

    }
}
