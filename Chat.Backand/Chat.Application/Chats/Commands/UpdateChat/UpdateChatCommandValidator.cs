namespace Chat.Application.Chats.Commands.UpdateChat;

public class UpdateChatCommandValidator : AbstractValidator<UpdateChatCommand>
{
    public UpdateChatCommandValidator()
    {
        RuleFor(updateChatCommand => updateChatCommand.UserId)
            .NotEqual(Guid.Empty);
        RuleFor(updateChatCommand => updateChatCommand.ChatId)
            .NotEqual(Guid.Empty);
        RuleFor(updateChatCommand => updateChatCommand.Title)
            .NotEmpty()
            .Length(3, 20)
            .WithName("Название чата");
    }
}
