namespace Chat.Application.Chats.Commands.CreateChat;

public class CreateChatCommand : IRequest<Guid>
{
    public Guid UserId { get; set; } = Guid.Empty;
    public string Title { get; set; } = string.Empty;
}
