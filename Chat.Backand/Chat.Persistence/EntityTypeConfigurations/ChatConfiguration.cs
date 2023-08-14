using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Chat.Persistence.EntityTypeConfigurations;

public class ChatConfiguration : IEntityTypeConfiguration<Domain.Chat>
{
    public void Configure(EntityTypeBuilder<Domain.Chat> builder)
    {
        builder.HasKey(chat => chat.Id);
        builder.HasIndex(chat => chat.Id).IsUnique();
        builder.Property(chat => chat.Title).HasMaxLength(20).IsRequired();
        builder.Property(chat => chat.DateCreateChat).IsRequired();

        builder.HasOne(chat => chat.User)
            .WithMany(user => user.UserChats)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(chat => chat.Members)
            .WithMany(users => users.MemberChats);
    }
}
