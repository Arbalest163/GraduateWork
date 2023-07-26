namespace Chat.Application.Chats.Commands.UpdateChat;

public class UpdateChatCommand : IRequest
{
    public Guid UserId { get; set; }
    public Guid ChatId { get; set; }
    public string Title { get; set; }

    public Guid? DeleteUserId { get; set; }
    public Guid? AddUserId { get; set; }
}
