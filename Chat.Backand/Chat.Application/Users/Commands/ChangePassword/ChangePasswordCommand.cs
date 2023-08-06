namespace Chat.Application.Users.Commands.ChangePassword;

public class ChangePasswordCommand : IRequest
{
    public Guid UserId { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
}
