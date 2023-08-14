using Chat.Domain.Interfaces;

namespace Chat.Application.Common.Models;

public class ReceiveMessageBase
{
    public string Date { get; set; }
    public string Text { get; set; }
}

public class ReceiveMessage : ReceiveMessageBase, IEntity
{
    public Guid Id { get; set; }
    public UserReceiveMessage User { get; set; }
    public string TimeSendMessage { get; set; }
}

public class UserReceiveMessage : Entity
{
    public string Avatar { get; set; }
    public string Nickname { get; set; }
}

public class InformationMessage : ReceiveMessageBase
{
}
