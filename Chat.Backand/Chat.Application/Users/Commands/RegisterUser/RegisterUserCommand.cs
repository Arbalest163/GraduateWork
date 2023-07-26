using System.ComponentModel;

namespace Chat.Application.Users.Commands.RegisterUser;

public class RegisterUserCommand : IRequest
{
    [DisplayName("Имя пользвоателя")]
    public string Username { get; set; } = string.Empty;
    public string Firstname { get; set; } = string.Empty;
    public string Lastname { get; set; } = string.Empty;
    public string Middlename { get; set; } = string.Empty;
    public DateTimeOffset? Birthday { get; set; }
    public string Nickname { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string ConfirmPassword { get; set; } = string.Empty;
}
