using Chat.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Chat.Persistence.EntityTypeConfigurations
{
    public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.HasKey(role => role.Id);
            builder.HasIndex(role => role.Id).IsUnique();
            builder.Property(x => x.Role).HasConversion<string>().IsRequired();
            builder.HasIndex(x => x.Role).IsUnique();
            builder.Property(x => x.RoleName).HasMaxLength(20).IsRequired();
        }
    }
}
