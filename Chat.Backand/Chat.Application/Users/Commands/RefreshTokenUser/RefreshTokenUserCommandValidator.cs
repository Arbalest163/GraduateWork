namespace Chat.Application.Users.Commands.RefreshTokenUser;

public class RefreshTokenUserCommandValidator : AbstractValidator<RefreshTokenUserCommand>
{
    public RefreshTokenUserCommandValidator()
    {
            RuleFor(x => x.RefreshToken).NotEmpty();
    }
}
