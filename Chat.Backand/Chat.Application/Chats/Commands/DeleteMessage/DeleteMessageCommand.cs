namespace Chat.Application.Chats.Commands.DeleteMessage;

public class DeleteMessageCommand : IRequest
{
    public Guid MessageId { get; set; }
}
