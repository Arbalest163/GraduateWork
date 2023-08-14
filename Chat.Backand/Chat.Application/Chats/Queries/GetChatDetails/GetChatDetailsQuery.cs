namespace Chat.Application.Chats.Queries.GetChatDetails;

public class GetChatDetailsQuery : IRequest<ChatDetailsVm>
{
    public Guid ChatId { get; set; }
}
