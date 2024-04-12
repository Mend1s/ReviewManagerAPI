using Microsoft.EntityFrameworkCore;
using ReviewManager.Core.Entities;

namespace ReviewManager.Infrastructure.Persistence;

public class ReviewManagerDbContext : DbContext
{
    public ReviewManagerDbContext(DbContextOptions options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<Book> Books { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ReviewManagerDbContext).Assembly);
    }
}
