namespace Chat.Application.Chats.Queries.GetMessageGroups;

public class GetMessageGroupsQuery : IRequest<MessageGroupsVm>
{
    public Guid ChatId { get; set; }
}
