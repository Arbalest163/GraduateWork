namespace Chat.Application.Chats.Queries.CheckChatAccess;

public class CheckChatAccessQuery : IRequest<bool>
{
    public Guid ChatId { get; set; }
}
