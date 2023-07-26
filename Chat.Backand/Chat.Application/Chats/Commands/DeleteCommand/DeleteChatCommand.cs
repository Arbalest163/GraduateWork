namespace Chat.Application.Chats.Commands.DeleteCommand;

public class DeleteChatCommand : IRequest
{
    public Guid UserId { get; set; }
    public Guid ChatId { get; set; }
}
