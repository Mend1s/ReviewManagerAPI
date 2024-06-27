using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReviewManager.Core.Entities;

namespace ReviewManager.Infrastructure.Persistence.Configurations;

public class BooksConfigurations : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.
            HasKey(e => e.Id);

        builder
            .Property(e => e.Title)
            .HasMaxLength(150)
            .IsRequired();

        builder
            .Property(e => e.Description)
            .HasMaxLength(500)
            .IsRequired();

        builder
            .Property(e => e.ISBN)
            .HasMaxLength(13)
            .IsRequired();

        builder
            .Property(e => e.Author)
            .HasMaxLength(100)
            .IsRequired();

        builder
            .Property(e => e.Publisher)
            .HasMaxLength(80)
            .IsRequired();

        builder.
            Property(e => e.Genre)
            .IsRequired();

        builder
            .Property(e => e.YearOfPublication)
            .HasMaxLength(4)
            .IsRequired();

        builder
            .Property(e => e.NumberOfPages)
            .HasMaxLength(5000)
            .IsRequired();

        builder
            .Property(e => e.CreateDate)
            .IsRequired();

        builder
            .Property(e => e.AverageGrade)
            .HasColumnType("decimal(5, 2)")
            .IsRequired();

        builder
            .Property(e => e.ImageUrl)
            .IsRequired()
            .HasMaxLength(4000)
            .HasColumnType("nvarchar(max)");

        builder
            .HasMany(b => b.Reviews)
            .WithOne(r => r.Book)
            .HasForeignKey(r => r.IdBook);
    }
}
