namespace Chat.Application.Common.Models;
public class ReceiveMessage : Entity
{
    public string Text { get; set; }
    public string Date { get; set; }
    public UserReceiveMessage User { get; set; }
    public string TimeSendMessage { get; set; }
}

public class UserReceiveMessage : Entity
{
    public string Avatar { get; set; }
    public string Nickname { get; set; }
}
