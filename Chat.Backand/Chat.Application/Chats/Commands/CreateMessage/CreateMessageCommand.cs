namespace Chat.Application.Chats.Commands.CreateMessage;

public class CreateMessageCommand : IRequest<string>
{
    public Guid UserId { get; set; }
    public Guid ChatId { get; set; }
    public string Message { get; set; }
}
