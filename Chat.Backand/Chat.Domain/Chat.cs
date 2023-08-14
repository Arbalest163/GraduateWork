namespace Chat.Domain;
public class Chat : Entity
{
    /// <summary>
    /// ��������� ����
    /// </summary>
    public string Title { get; set; }
    /// <summary>
    /// ������� ����
    /// </summary>
    public string ChatLogo { get; set; }
    /// <summary>
    /// ������������, ��������� ���
    /// </summary>
    public virtual User User { get; set; }
    /// <summary>
    /// ��������� ����
    /// </summary>
    public virtual List<Message> Messages { get; set; } = new List<Message>();
    /// <summary>
    /// ��������� ����
    /// </summary>
    public virtual List<User> Members { get; set; } = new List<User>();
    /// <summary>
    /// ���� �������� ����
    /// </summary>
    public virtual DateTimeOffset DateCreateChat { get; set; }
    /// <summary>
    /// ������� ��������� ����
    /// </summary>
    public virtual bool IsActive { get; set; }
}