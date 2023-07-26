using Chat.Application.Interfaces;
using Chat.Domain;
using Chat.Persistence.EntityTypeConfigurations;
using Microsoft.EntityFrameworkCore;

namespace Chat.Persistence;

public class ChatDbContext : DbContext, IChatDbContext
{
    public DbSet<User> Users => Set<User>();

    public DbSet<Domain.Chat> Chats => Set<Domain.Chat>();

    public DbSet<Message> Messages => Set<Message>();

    public DbSet<UserRole> UserRoles => Set<UserRole>();

    public ChatDbContext(DbContextOptions<ChatDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfiguration(new UserConfiguration());
        builder.ApplyConfiguration(new ChatConfiguration());
        builder.ApplyConfiguration(new MessageConfiguration());
        builder.ApplyConfiguration(new UserRoleConfiguration());
    }
}
