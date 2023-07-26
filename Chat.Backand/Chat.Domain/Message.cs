namespace Chat.Domain;
public class Message : Entity
{
    public virtual Chat Chat { get; set; }
    public virtual string Text { get; set; }
    public virtual User User { get; set; }
    public virtual DateTimeOffset DateSendMessage { get; set; }
}
