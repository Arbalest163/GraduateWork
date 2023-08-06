using FluentValidation;

namespace Chat.Application.Users.Commands.EditUser;

public class EditUserCommandValidator : AbstractValidator<EditUserCommand>
{
    public EditUserCommandValidator()
    {
        RuleFor(editUserCommand => editUserCommand.Firstname)
            .MaximumLength(20)
            .WithName("Имя");
        RuleFor(editUserCommand => editUserCommand.Lastname)
            .MaximumLength(20)
            .WithName("Фамилия");
        RuleFor(editUserCommand => editUserCommand.Middlename)
            .MaximumLength(20)
            .WithName("Отчество");
        RuleFor(editUserCommand => editUserCommand.Birthday)
            .NotEqual(x => DateTimeOffset.MinValue)
            .WithName("Дата рождения");
    }
}
