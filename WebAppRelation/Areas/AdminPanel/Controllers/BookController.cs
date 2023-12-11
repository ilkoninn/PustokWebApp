
using Microsoft.AspNetCore.Authorization;
using WebAppRelation.Models;

namespace WebAppRelation.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class BookController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;
        public BookController(AppDbContext db, IWebHostEnvironment environment)
        {
            _db = db;
            _env = environment;
        }


        // <--- Table Section --->
        [Authorize(Roles = "Admin, Moderator")]
        public async Task<IActionResult> Table()
        {
            AdminVM admin = new AdminVM();
            admin.Brands = await _db.Brands.ToListAsync();
            admin.Authors = await _db.Authors.ToListAsync();
            admin.Categories = await _db.Categories.ToListAsync();
            admin.Books = await _db.Books
                .Include(x => x.Tags)
                .ThenInclude(x => x.Tag)
                .Include(x => x.BookImages)
                .ToListAsync();
            admin.Tags = await _db.Tags.ToListAsync();

            return View(admin);
        }

        // <--- Create Section --->
        [HttpGet]
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CreateBookVM createBookVM)
        {
            createBookVM.Authors = await _db.Authors.ToListAsync();
            createBookVM.Categories = await _db.Categories.ToListAsync();
            createBookVM.Tags = await _db.Tags.ToListAsync();
            createBookVM.Brands = await _db.Brands.ToListAsync();

            if (createBookVM.CategoryId == null || createBookVM.BrandId == null || createBookVM.AuthorId == null)
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
                AuthorId = createBookVM.AuthorId,
                CategoryId = createBookVM.CategoryId,
                BrandId = createBookVM.BrandId,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                BookImages = new List<BookImages>()
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

            if (createBookVM.MainImage != null)
            {
                if (!createBookVM.MainImage.CheckType("image/"))
                {
                    ModelState.AddModelError("ImageFile", "You can upload only images");
                    return View(createBookVM);
                }
                if (!createBookVM.MainImage.CheckLong(2097152))
                {
                    ModelState.AddModelError("ImageFile", "The maximum size of image is 2MB!");
                    return View(createBookVM);
                }

                if (!ModelState.IsValid)
                {
                    return View(createBookVM);
                }

                BookImages bookImage = new BookImages
                {
                    IsPrime = true,
                    Book = newBook,
                    ImgUrl = createBookVM.MainImage.Upload(_env.WebRootPath, @"\Upload\BookImages\")
                };

                newBook.BookImages.Add(bookImage);
            }
            else
            {
                ModelState.AddModelError("MainImage", "You must be upload a main image!");
                return View(createBookVM);
            }

            if (createBookVM.HoverImage != null)
            {
                if (!createBookVM.HoverImage.CheckType("image/"))
                {
                    ModelState.AddModelError("ImageFile", "You can upload only images");
                    return View(createBookVM);
                }
                if (!createBookVM.HoverImage.CheckLong(2097152))
                {
                    ModelState.AddModelError("ImageFile", "The maximum size of image is 2MB!");
                    return View(createBookVM);
                }

                if (!ModelState.IsValid)
                {
                    return View(createBookVM);
                }

                BookImages bookImage = new BookImages
                {
                    IsPrime = false,
                    Book = newBook,
                    ImgUrl = createBookVM.HoverImage.Upload(_env.WebRootPath, @"\Upload\BookImages\")
                };

                newBook.BookImages.Add(bookImage);
            }
            else
            {
                ModelState.AddModelError("HoverImage", "You must be upload a hover image!");
                return View(createBookVM);
            }

            if (createBookVM.Images != null)
            {
                foreach (var item in createBookVM.Images)
                {
                    if (!item.CheckType("image/"))
                    {
                        ModelState.AddModelError("ImageFile", "You can upload only images");
                        return View(createBookVM);
                    }
                    if (!item.CheckLong(2097152))
                    {
                        ModelState.AddModelError("ImageFile", "The maximum size of image is 2MB!");
                        return View(createBookVM);
                    }

                    if (!ModelState.IsValid)
                    {
                        return View(createBookVM);
                    }

                    BookImages bookImage = new BookImages
                    {
                        IsPrime = null,
                        Book = newBook,
                        ImgUrl = item.Upload(_env.WebRootPath, @"\Upload\BookImages\")
                    };

                    newBook.BookImages.Add(bookImage);
                }
            }

            _db.Books.Add(newBook);
            await _db.SaveChangesAsync();
            return RedirectToAction("Table");
        }

        // <--- Update Section --->
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int Id)
        {
            Book Book = await _db.Books
                .Include(x => x.Tags)
                .ThenInclude(x => x.Tag)
                .Include(x => x.BookImages)
                .FirstOrDefaultAsync(p => p.Id == Id);

            ICollection<Tag> tags = await _db.Tags.ToListAsync();

            List<int> tagIds = new();

            foreach (Tag tag in tags)
                foreach (var item in Book.Tags)
                    if (tag.Id == item.TagId)
                        tagIds.Add(tag.Id);

            UpdateBookVM updateBookVM = new UpdateBookVM
            {
                Title = Book.Title,
                Description = Book.Description,
                BookCode = Book.BookCode,
                Price = Book.Price,
                CategoryId = Book.CategoryId,
                BrandId = Book.BrandId,
                AuthorId = Book.AuthorId,
                Categories = await _db.Categories.ToListAsync(),
                Brands = await _db.Brands.ToListAsync(),
                Authors = await _db.Authors.ToListAsync(),
                Tags = tags,
                TagIds = tagIds,
                BookImageVMs = new List<BookImageVM>(),
            };


            foreach (var item in Book.BookImages)
            {
                BookImageVM bookImageVM = new BookImageVM
                {
                    Id = item.Id,
                    IsPrime = item.IsPrime,
                    ImgUrl = item.ImgUrl,
                };

                updateBookVM.BookImageVMs.Add(bookImageVM);
            }

            return View(updateBookVM);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(UpdateBookVM updateBookVM)
        {
            Book oldBook = await _db.Books
                .Include(x => x.Tags)
                .ThenInclude(x => x.Tag)
                .Include(x => x.BookImages)
                .FirstOrDefaultAsync(x => x.Id == updateBookVM.Id);

            oldBook.Title = updateBookVM.Title;
            oldBook.Description = updateBookVM.Description;
            oldBook.BookCode = updateBookVM.BookCode;
            oldBook.Price = updateBookVM.Price;
            oldBook.AuthorId = updateBookVM.AuthorId;
            oldBook.CategoryId = updateBookVM.CategoryId;
            oldBook.BrandId = updateBookVM.BrandId;
            oldBook.CreatedDate = oldBook.CreatedDate;
            oldBook.UpdatedDate = DateTime.Now;

            oldBook.Tags.Clear();

            if (updateBookVM.TagIds != null)
            {
                foreach (var tagId in updateBookVM.TagIds)
                {
                    BookTag bookTag = new BookTag()
                    {
                        Book = oldBook,
                        TagId = tagId,
                    };

                    oldBook.Tags.Add(bookTag);
                }
            }

            if (updateBookVM.MainImage != null)
            {
                if (!updateBookVM.MainImage.CheckType("image/"))
                {
                    ModelState.AddModelError("ImageFile", "You can upload only images");
                    return View(updateBookVM);
                }
                if (!updateBookVM.MainImage.CheckLong(2097152))
                {
                    ModelState.AddModelError("ImageFile", "The maximum size of image is 2MB!");
                    return View(updateBookVM);
                }

                if (!ModelState.IsValid)
                {
                    return View(updateBookVM);
                }

                var existMainPhoto = oldBook.BookImages.FirstOrDefault(p => p.IsPrime == true);
                existMainPhoto.ImgUrl.Delete(_env.WebRootPath, @"\Upload\BookImages\");
                oldBook.BookImages.Remove(existMainPhoto);

                BookImages bookImage = new BookImages
                {
                    IsPrime = true,
                    Book = oldBook,
                    ImgUrl = updateBookVM.MainImage.Upload(_env.WebRootPath, @"\Upload\BookImages\")
                };

                oldBook.BookImages.Add(bookImage);
            }

            if (updateBookVM.HoverImage != null)
            {
                if (!updateBookVM.HoverImage.CheckType("image/"))
                {
                    ModelState.AddModelError("ImageFile", "You can upload only images");
                    return View(updateBookVM);
                }
                if (!updateBookVM.HoverImage.CheckLong(2097152))
                {
                    ModelState.AddModelError("ImageFile", "The maximum size of image is 2MB!");
                    return View(updateBookVM);
                }

                if (!ModelState.IsValid)
                {
                    return View(updateBookVM);
                }

                var existMainPhoto = oldBook.BookImages.FirstOrDefault(p => p.IsPrime == false);
                existMainPhoto.ImgUrl.Delete(_env.WebRootPath, @"\Upload\BookImages\");
                oldBook.BookImages.Remove(existMainPhoto);

                BookImages bookImage = new BookImages
                {
                    IsPrime = false,
                    Book = oldBook,
                    ImgUrl = updateBookVM.HoverImage.Upload(_env.WebRootPath, @"\Upload\BookImages\")
                };

                oldBook.BookImages.Add(bookImage);
            }

            if (updateBookVM.ImageIds == null)
            {
                oldBook.BookImages.RemoveAll(x => x.IsPrime == null);
            }
            else
            {
                List<BookImages> removeList = oldBook.BookImages.Where(pt => !updateBookVM.ImageIds.Contains(pt.Id) && pt.IsPrime == null).ToList();

                if (removeList.Count > 0)
                {
                    foreach (var item in removeList)
                    {
                        oldBook.BookImages.Remove(item);
                        item.ImgUrl.Delete(_env.WebRootPath, @"\Upload\BookImages\");
                    }
                }
            }

            TempData["Error"] = "";
            if (updateBookVM.Images != null)
            {
                foreach (IFormFile imgFile in updateBookVM.Images)
                {
                    if (!imgFile.CheckType("image/"))
                    {
                        TempData["Error"] += $"{imgFile.FileName} uygun tipde deyil ";
                        continue;
                    }
                    if (!imgFile.CheckLong(2097152))
                    {
                        TempData["Error"] += $"{imgFile.FileName} file-nin olcusu coxdur";
                        continue;
                    }

                    BookImages bookImages = new BookImages()
                    {
                        IsPrime = null,
                        BookId = oldBook.Id,
                        ImgUrl = imgFile.Upload(_env.WebRootPath, "/Upload/BookImages/")
                    };
                    oldBook.BookImages.Add(bookImages);
                }
            }


            await _db.SaveChangesAsync();

            return RedirectToAction("Table");
        }

        // <--- Detail Section --->

        [HttpGet]
        [Authorize(Roles = "Admin, Moderator")]
        public async Task<IActionResult> Detail(int Id)
        {
            Book Book = await _db.Books
                .Include(x => x.Tags)
                .ThenInclude(x => x.Tag)
                .Include(x => x.BookImages)
                .FirstOrDefaultAsync(p => p.Id == Id);

            ICollection<Tag> tags = await _db.Tags.ToListAsync();

            List<int> tagIds = new();

            foreach (Tag tag in tags)
                foreach (var item in Book.Tags)
                    if (tag.Id == item.TagId)
                        tagIds.Add(tag.Id);

            UpdateBookVM updateBookVM = new UpdateBookVM
            {
                Title = Book.Title,
                Description = Book.Description,
                BookCode = Book.BookCode,
                Price = Book.Price,
                CategoryId = Book.CategoryId,
                BrandId = Book.BrandId,
                AuthorId = Book.AuthorId,
                Categories = await _db.Categories.ToListAsync(),
                Brands = await _db.Brands.ToListAsync(),
                Authors = await _db.Authors.ToListAsync(),
                Tags = tags,
                TagIds = tagIds,
                BookImageVMs = new List<BookImageVM>(),
            };


            foreach (var item in Book.BookImages)
            {
                BookImageVM bookImageVM = new BookImageVM
                {
                    Id = item.Id,
                    IsPrime = item.IsPrime,
                    ImgUrl = item.ImgUrl,
                };

                updateBookVM.BookImageVMs.Add(bookImageVM);
            }

            return View(updateBookVM);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int Id)
        {
            Book oldBook = await _db.Books
                .Include(x => x.BookImages)
                .FirstOrDefaultAsync(x => x.Id == Id);

            _db.BookImages.RemoveRange(oldBook.BookImages);
            _db.Books.Remove(oldBook);

            foreach (var item in oldBook.BookImages)
            {
                item.ImgUrl.Delete(_env.WebRootPath, @"\Upload\BookImages\");
            }

            await _db.SaveChangesAsync();
            return RedirectToAction("Table");
        }

    }
}
