using Chat.Application.Common.Exceptions;

namespace Chat.Application.Chats.Commands.DeleteCommand;

public class DeleteChatCommandHandler
    : IRequestHandler<DeleteChatCommand>
{
    private readonly IChatDbContext _dbContext;
    private readonly IChatUserPrincipal _userPrincipal;

    public DeleteChatCommandHandler(IChatDbContext dbContext, IChatUserPrincipal userPrincipal)
    {
        _dbContext = dbContext;
        _userPrincipal = userPrincipal;
    }

    public async Task Handle(DeleteChatCommand request,
        CancellationToken cancellationToken)
    {
        var chat = await _dbContext.Chats
            .Include(x => x.User)
            .Where(chat => chat.Id == request.ChatId)
            .FirstOrDefaultAsync(cancellationToken);

        if (chat == null)
        {
            throw new NotFoundException(nameof(Domain.Chat), request.ChatId);
        }

        if(chat.User.Id == _userPrincipal.UserId || _userPrincipal.Role == Role.Admin)
        {
            _dbContext.Chats.Remove(chat);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
