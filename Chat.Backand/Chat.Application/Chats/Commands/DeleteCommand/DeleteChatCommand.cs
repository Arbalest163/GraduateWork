namespace Chat.Application.Chats.Commands.DeleteCommand;

public class DeleteChatCommand : IRequest
{
    public Guid ChatId { get; set; }
}
