

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace WebAppRelation.DAL;

public class AppDbContext:IdentityDbContext<AppUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options):base(options) { }
    public DbSet<Blog> Blogs { get; set; }
    public DbSet<BookImages> BookImages { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<Brand> Brands { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Author> Authors { get; set; }
    public DbSet<BlogTag> BlogTags { get; set; }
    public DbSet<BookTag> BookTags { get; set; }
}
