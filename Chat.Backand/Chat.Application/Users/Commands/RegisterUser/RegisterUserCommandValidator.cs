namespace Chat.Application.Users.Commands.RegisterUser;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(registerUserCommand => registerUserCommand.Username)
            .NotEmpty()
            .MaximumLength(20)
            .WithName("Логин");
        RuleFor(registerUserCommand => registerUserCommand.Nickname)
            .NotEmpty()
            .MaximumLength(20)
            .WithName("Ник");
        RuleFor(registerUserCommand => registerUserCommand.Password)
            .NotEmpty()
            .Length(6, 20)
            .WithName("Пароль");
        RuleFor(registerUserCommand => registerUserCommand.ConfirmPassword)
            .NotEmpty()
            .Equal(x => x.Password)
            .WithMessage("Пароли должны совпадать.")
            .WithName("Подтверждение пароля");
    }
}
