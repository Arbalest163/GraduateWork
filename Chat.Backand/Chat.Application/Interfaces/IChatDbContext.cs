namespace Chat.Application.Interfaces;

public interface IChatDbContext
{
    DbSet<User> Users { get; }
    DbSet<Domain.Chat> Chats { get; }
    DbSet<Message> Messages { get; }
    DbSet<UserRole> UserRoles { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
