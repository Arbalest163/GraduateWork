using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Chat.Domain;

namespace Chat.Persistence.EntityTypeConfigurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(user => user.Id);
        builder.HasIndex(user => user.Id).IsUnique();
        builder.Property(user => user.UserName).HasMaxLength(20).IsRequired();
        builder.HasIndex(user => user.UserName).IsUnique();
        builder.Property(user => user.Firstname).HasMaxLength(20);
        builder.Property(user => user.Lastname).HasMaxLength(20);
        builder.Property(user => user.Middlename).HasMaxLength(20);
        builder.Property(user => user.Nickname).HasMaxLength(20).IsRequired();
        builder.HasIndex(user => user.Nickname).IsUnique();
        builder.Property(user => user.Birthday);
        builder.Property(user => user.PasswordHash).IsRequired();

        builder.HasMany(x => x.UserChats).WithOne(x => x.User);
        builder.HasMany(x => x.MemberChats).WithMany(x => x.Users);
    }
}
