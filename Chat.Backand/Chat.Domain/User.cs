namespace Chat.Domain;
public class User : Entity
{
    public virtual string UserName { get; set; }
    public virtual string PasswordHash { get; set; }
    public virtual string Firstname { get; set; } = string.Empty;
    public virtual string Lastname { get; set; } = string.Empty;
    public virtual string Middlename { get; set; } = string.Empty;
    public virtual DateTimeOffset? Birthday { get; set; }
    public virtual string Nickname { get; set; }
    public virtual UserRole UserRole { get; set; }
    public virtual string Avatar { get; set; } = string.Empty;
    /// <summary>
    /// Чаты, созданные пользователем
    /// </summary>
    public virtual IList<Chat> UserChats { get; set; } = new List<Chat>();
    /// <summary>
    /// Чаты, в которых пользователь присутствует как участник
    /// </summary>
    public virtual IList<Chat> MemberChats { get; set; } = new List<Chat>();
    /// <summary>
    /// Все сообщения пользователя
    /// </summary>
    public virtual IList<Message> Messages { get; set; } = new List<Message>();
}
