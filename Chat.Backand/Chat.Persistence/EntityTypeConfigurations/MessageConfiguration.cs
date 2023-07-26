using Chat.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Chat.Persistence.EntityTypeConfigurations;

public class MessageConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.HasKey(message => message.Id);
        builder.HasIndex(message => message.Id).IsUnique();
        builder.Property(message => message.Text).HasMaxLength(500).IsRequired();
        builder.Property(message => message.DateSendMessage).IsRequired();

        builder.HasOne(message => message.Chat)
            .WithMany(chat => chat.Messages)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(message => message.User)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
