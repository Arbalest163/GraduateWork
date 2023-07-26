namespace Chat.Application.Users.Commands.LoginUser;

public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
{
    public LoginUserCommandValidator()
    {
        RuleFor(registerUserCommand => registerUserCommand.Login)
            .NotEmpty()
            .MaximumLength(20)
            .WithName("Логин");
        RuleFor(registerUserCommand => registerUserCommand.Password)
            .NotEmpty()
            .MaximumLength(20)
            .WithName("Пароль");
    }
}
