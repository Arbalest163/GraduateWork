using Chat.Application.Common;
using Chat.Application.Common.Exceptions;
using Chat.Application.Common.Models;
using Chat.Application.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace Chat.Application.Chats.Commands.CreateMessage;

public class CreateMessageCommandHandler
 : IRequestHandler<CreateMessageCommand, string>
{
    private readonly IChatDbContext _dbContext;
    private readonly IChatUserPrincipal _userPrincipal;
    private readonly IHubContext<ChatHub, IChatClient> _hubContext;

    public CreateMessageCommandHandler(IChatDbContext dbContext, IChatUserPrincipal userPrincipal, IHubContext<ChatHub, IChatClient> hubContext)
    {
        _dbContext = dbContext;
        _userPrincipal = userPrincipal;
        _hubContext = hubContext;
    }

    public async Task<string> Handle(CreateMessageCommand request,
        CancellationToken cancellationToken)
    {
        var chat = await _dbContext.Chats.Include(x => x.Users).FirstOrDefaultAsync(x => x.Id == request.ChatId);
        
        if (chat is null)
        {
            throw new NotFoundException(nameof(Domain.Chat), request.ChatId);
        }

        var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == _userPrincipal.UserId) ?? throw new UnauthorizedAccessException();

        var message = new Message
        {
            User = user,
            Text = request.Message,
            DateSendMessage = DateTimeOffset.UtcNow,
        };

        chat.Messages.Add(message);

        if(!chat.Users.Any(x => x.Id == user.Id))
        {
            chat.Users.Add(user);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);

        var receiveMessage = new ReceiveMessage
        {
            Id = message.Id,
            User = new UserReceiveMessage
            {
                Id = user.Id,
                Nickname = user.Nickname,
                Avatar = Converter.CreateBase64File(user.Avatar),
            },
            Text = message.Text,
            Date = message.DateSendMessage.LocalDateTime.ToString("d MMM"),
            TimeSendMessage = message.DateSendMessage.LocalDateTime.ToString("HH:mm:ss"),
        };

        await _hubContext.Clients.Group(chat.Id.ToString()).ReceiveMessage(receiveMessage);

        return message.Text;
    }
}
