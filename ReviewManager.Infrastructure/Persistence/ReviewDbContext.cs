using Microsoft.EntityFrameworkCore;
using ReviewManager.Core.Entities;

namespace ReviewManager.Infrastructure.Persistence;

public class ReviewDbContext : DbContext
{
    public ReviewDbContext() { }
    public ReviewDbContext(DbContextOptions<ReviewDbContext> options) : base ( options ) { }
    public DbSet<User> Users { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<Book> Books { get; set; }

    //protected override void OnModelCreating(ModelBuilder modelBuilder)
    //{
    //    modelBuilder.ApplyConfigurationsFromAssembly(typeof(ReviewDbContext).Assembly);
    //}
}
