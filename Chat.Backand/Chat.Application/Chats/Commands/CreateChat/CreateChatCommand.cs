namespace Chat.Application.Chats.Commands.CreateChat;

public class CreateChatCommand : IRequest<Guid>
{
    public string Title { get; set; } = string.Empty;
    public string ChatLogo { get; set; } = string.Empty;
}
