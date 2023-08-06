namespace Chat.Application.Chats.Queries.GetChatInfo;

public class GetChatInfoQuery : IRequest<ChatInfoVm>
{
    public Guid ChatId { get; set; }
}
