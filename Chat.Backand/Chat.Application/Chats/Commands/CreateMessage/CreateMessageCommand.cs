namespace Chat.Application.Chats.Commands.CreateMessage;

public class CreateMessageCommand : IRequest<string>
{
    public Guid ChatId { get; set; }
    public string Message { get; set; }
}
