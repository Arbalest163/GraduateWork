namespace Chat.Domain;
public class GroupChat : Entity
{
    public virtual string Title { get; set; }
    public virtual ICollection<Chat> Chats { get; set; }
}