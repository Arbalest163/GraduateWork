namespace Chat.Domain;

public class UserRole : Entity
{
    public virtual Role Role { get; set; }
    public virtual string RoleName { get; set; } = string.Empty;
}

public enum Role
{
    Admin = 0,
    Support = 1,
    User = 2
}
