namespace Chat.Application.Users.Commands.LoginUser;

public class LoginUserCommand : IRequest<Token>
{
    public string Login { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
