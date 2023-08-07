namespace Chat.Domain;
public class User : Entity
{
    /// <summary>
    /// Имя пользователя для входа
    /// </summary>
    public virtual string UserName { get; set; }
    /// <summary>
    /// Пароль
    /// </summary>
    public virtual string PasswordHash { get; set; }
    /// <summary>
    /// Имя
    /// </summary>
    public virtual string Firstname { get; set; } = string.Empty;
    /// <summary>
    /// Фамилия
    /// </summary>
    public virtual string Lastname { get; set; } = string.Empty;
    /// <summary>
    /// Отчество
    /// </summary>
    public virtual string Middlename { get; set; } = string.Empty;
    /// <summary>
    /// Дата рождения
    /// </summary>
    public virtual DateTimeOffset? Birthday { get; set; }
    /// <summary>
    /// Никнейм
    /// </summary>
    public virtual string Nickname { get; set; }
    /// <summary>
    /// Роль пользователя
    /// </summary>
    public virtual UserRole UserRole { get; set; }
    /// <summary>
    /// Аватар пользователя
    /// </summary>
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
