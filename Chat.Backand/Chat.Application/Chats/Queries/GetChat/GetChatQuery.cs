namespace Chat.Application.Chats.Queries.GetChat;

public class GetChatQuery : IRequest<ChatVm>
{
    public Guid ChatId { get; set; }
}
