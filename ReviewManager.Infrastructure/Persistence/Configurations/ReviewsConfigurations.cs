using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReviewManager.Core.Entities;

namespace ReviewManager.Infrastructure.Persistence.Configurations;

public class ReviewsConfigurations : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Note)
            .IsRequired();

        builder.Property(e => e.IdUser)
            .IsRequired();

        builder.Property(e => e.IdBook)
            .IsRequired();

        builder.Property(e => e.Description);

        builder.Property(e => e.CreateDate)
            .IsRequired();

        builder.HasOne(r => r.User)
            .WithMany(u => u.Reviews)
            .HasForeignKey(r => r.IdUser);
    }
}