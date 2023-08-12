namespace Chat.Application.Chats.Queries.GetEditChat;

public class GetEditChatQuery : IRequest<EditChatVm>
{
    public Guid ChatId { get; set; }
}
