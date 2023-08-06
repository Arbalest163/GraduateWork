namespace Chat.Application.Users.Commands.EditUser;

public class EditUserCommand : IRequest
{
    public Guid UserId { get; set; }
    public string Firstname { get; set; } = string.Empty;
    public string Lastname { get; set; } = string.Empty;
    public string Middlename { get; set; } = string.Empty;
    public DateTimeOffset? Birthday { get; set; }
    public string Avatar { get; set; }
}
