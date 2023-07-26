using Chat.Application.Common.Exceptions;

namespace Chat.Application.Chats.Commands.DeleteCommand;

public class DeleteChatCommandHandler
    : IRequestHandler<DeleteChatCommand>
{
    private readonly IChatDbContext _dbContext;

    public DeleteChatCommandHandler(IChatDbContext dbContext) =>
        _dbContext = dbContext;

    public async Task Handle(DeleteChatCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == request.UserId);
        if (user == null)
        {
            throw new UnauthorizedAccessException();
        }
        var entity = await _dbContext.Chats
            .Where(chat => chat.Id == request.ChatId)
            .Where(chat => chat.User.Id == user.Id || user.UserRole.Role == Role.Admin)
            .FirstOrDefaultAsync(cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Domain.Chat), request.ChatId);
        }

        _dbContext.Chats.Remove(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
