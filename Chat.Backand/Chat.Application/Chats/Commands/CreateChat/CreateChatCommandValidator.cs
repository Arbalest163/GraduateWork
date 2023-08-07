namespace Chat.Application.Chats.Commands.CreateChat;

public class CreateChatCommandValidator : AbstractValidator<CreateChatCommand>
{
    public CreateChatCommandValidator()
    {
        RuleFor(createChatCommand =>
            createChatCommand.Title).NotEmpty().Length(3, 20).WithName("Название чата");
    }
}
