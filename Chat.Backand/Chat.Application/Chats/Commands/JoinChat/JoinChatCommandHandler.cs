using Chat.Application.Common.Models;
using Chat.Application.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace Chat.Application.Chats.Commands.JoinChat;

public class JoinChatCommandHandler
    : IRequestHandler<JoinChatCommand>
{
    private readonly IChatDbContext _chatDbContext;
    private readonly IChatUserPrincipal _userPrincipal;
    private readonly IHubContext<ChatHub, IChatClient> _hubContext;

    public JoinChatCommandHandler(IChatDbContext chatDbContext, IChatUserPrincipal userPrincipal, IHubContext<ChatHub, IChatClient> hubContext)
    {
        _chatDbContext = chatDbContext;
        _userPrincipal = userPrincipal;
        _hubContext = hubContext;
    }

    public async Task Handle(JoinChatCommand request, CancellationToken cancellationToken)
    {
        var chat = await _chatDbContext.Chats
            .Include(c => c.Members)
            .Where(c => c.Id == request.ChatId)
            .FirstOrDefaultAsync(cancellationToken);

        var user = await _chatDbContext.Users
            .Where(u => u.Id == _userPrincipal.UserId)
            .FirstOrDefaultAsync(cancellationToken);

        if (chat is null || user is null)
        {
            throw new UnauthorizedAccessException();
        }

        chat.Members.Add(user);
        await _chatDbContext.SaveChangesAsync(cancellationToken);

        var infMessage = new InformationMessage
        {
            Date = DateTime.Now.ToString("d MMM"),
            Text = $"{user.Nickname} присоеденился к чату."
        };

        await _hubContext.Clients.Group(chat.Id.ToString()).ReceiveInformationMessage(infMessage);
    }
}
