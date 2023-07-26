namespace Chat.Application.Chats.Commands.AddUserInChat;

public class AddUserInChatCommand : IRequest
{
    public Guid UserId { get; set; }
    public Guid ChatId { get; set; }
}
