namespace Chat.Application.Chats.Commands.UpdateChat;

public class UpdateChatCommand : IRequest
{
    public Guid ChatId { get; set; }
    public string Title { get; set; }
    public string ChatLogo { get; set; }

    public Guid? DeleteUserId { get; set; }
    public Guid? AddUserId { get; set; }
}
