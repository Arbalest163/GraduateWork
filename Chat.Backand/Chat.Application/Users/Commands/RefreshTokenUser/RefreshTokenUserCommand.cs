using Chat.Application.Users.Commands.LoginUser;

namespace Chat.Application.Users.Commands.RefreshTokenUser;

public class RefreshTokenUserCommand : IRequest<Token>
{
    public string RefreshToken { get; set; } = string.Empty;
}
