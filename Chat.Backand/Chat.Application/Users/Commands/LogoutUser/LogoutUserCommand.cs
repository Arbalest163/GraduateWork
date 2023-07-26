namespace Chat.Application.Users.Commands.LogoutUser;

public class LogoutUserCommand : IRequest
{
    public string UserId { get; set; }
}
