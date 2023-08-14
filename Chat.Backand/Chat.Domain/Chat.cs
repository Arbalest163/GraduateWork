namespace Chat.Domain;
public class Chat : Entity
{
    /// <summary>
    /// Заголовок чата
    /// </summary>
    public string Title { get; set; }
    /// <summary>
    /// Логотип чата
    /// </summary>
    public string ChatLogo { get; set; }
    /// <summary>
    /// Пользователь, создавший чат
    /// </summary>
    public virtual User User { get; set; }
    /// <summary>
    /// Сообщения чата
    /// </summary>
    public virtual List<Message> Messages { get; set; } = new List<Message>();
    /// <summary>
    /// Участники чата
    /// </summary>
    public virtual List<User> Members { get; set; } = new List<User>();
    /// <summary>
    /// Дата создания чата
    /// </summary>
    public virtual DateTimeOffset DateCreateChat { get; set; }
    /// <summary>
    /// Признак активного чата
    /// </summary>
    public virtual bool IsActive { get; set; }
}