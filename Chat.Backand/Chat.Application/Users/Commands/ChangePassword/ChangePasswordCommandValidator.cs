namespace Chat.Application.Users.Commands.ChangePassword;

public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordCommandValidator()
    {
        RuleFor(changePasswordCommand => changePasswordCommand.Password)
            .NotEmpty()
            .Length(6, 20)
            .WithName("Пароль");
        RuleFor(changePasswordCommand => changePasswordCommand.ConfirmPassword)
            .NotEmpty()
            .Equal(x => x.Password)
            .WithMessage("Пароли должны совпадать.")
            .WithName("Подтверждение пароля");
    }
}
