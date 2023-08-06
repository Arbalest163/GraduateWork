namespace Chat.Application.Chats.Queries.GetMessageGroups;

public class MessageGroupDto
{
    public string Date { get; set; }
    public IList<MessageLookupDto> Messages { get; set; }
}
