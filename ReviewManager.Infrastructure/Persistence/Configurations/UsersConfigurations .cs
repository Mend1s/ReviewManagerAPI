using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReviewManager.Core.Entities;

namespace ReviewManager.Infrastructure.Persistence.Configurations;

public class UsersConfigurations : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder
            .HasKey(u => u.Id);

        builder
            .Property(u => u.Name)
            .HasMaxLength(60)
            .IsRequired();

        builder
            .Property(u => u.Email)
            .HasMaxLength(50)
            .IsRequired();
    }
}
