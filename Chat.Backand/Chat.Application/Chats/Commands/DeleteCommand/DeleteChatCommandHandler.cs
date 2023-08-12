using Chat.Application.Common.Exceptions;
using Chat.Application.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace Chat.Application.Chats.Commands.DeleteCommand;

public class DeleteChatCommandHandler
    : IRequestHandler<DeleteChatCommand>
{
    private readonly IChatDbContext _dbContext;
    private readonly IChatUserPrincipal _userPrincipal;
    private readonly IHubContext<ChatHub, IChatClient> _hubContext;

    public DeleteChatCommandHandler(IChatDbContext dbContext, IChatUserPrincipal userPrincipal, IHubContext<ChatHub, IChatClient> hubContext)
    {
        _dbContext = dbContext;
        _userPrincipal = userPrincipal;
        _hubContext = hubContext;
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
            await _hubContext.Clients.All.OnChatCountChange();
        }
    }
}
