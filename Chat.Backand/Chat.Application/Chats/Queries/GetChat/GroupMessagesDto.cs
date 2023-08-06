namespace Chat.Application.Chats.Queries.GetChat;

public class GroupMessagesDto
{
    public string Date { get; set; }
    public IList<ChatMessageDto> Messages { get; set; }
}
