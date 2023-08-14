namespace Chat.Application.Chats.Commands.JoinChat;

public class JoinChatCommand : IRequest
{
    public Guid ChatId { get; set; }
}
