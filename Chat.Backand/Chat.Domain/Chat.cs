namespace Chat.Domain;
public class Chat : Entity
{
    public string Title { get; set; }
    /// <summary>
    /// Пользователь, создавший чат
    /// </summary>
    public virtual User User { get; set; }
    public virtual List<Message> Messages { get; set; } = new List<Message>();
    public virtual List<User> Users { get; set; } = new List<User>();
    public virtual DateTimeOffset DateCreateChat { get; set; }
    public virtual bool IsActive { get; set; }
}