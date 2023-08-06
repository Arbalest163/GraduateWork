namespace Chat.Application.Users.Commands.RegisterUser;

public class RegisterUserCommand : IRequest
{
    public string Username { get; set; } = string.Empty;
    public string Nickname { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string ConfirmPassword { get; set; } = string.Empty;
}
