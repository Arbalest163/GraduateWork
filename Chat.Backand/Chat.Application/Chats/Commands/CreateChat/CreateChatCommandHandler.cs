namespace Chat.Application.Chats.Commands.CreateChat;

public class CreateChatCommandHandler
 : IRequestHandler<CreateChatCommand, Guid>
{
    private readonly IChatDbContext _dbContext;

    public CreateChatCommandHandler(IChatDbContext dbContext) =>
        _dbContext = dbContext;

    public async Task<Guid> Handle(CreateChatCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users.FirstAsync(x => x.Id == request.UserId, cancellationToken);
        var chat = new Domain.Chat
        {
            User = user,
            Users = new List<User>() { user },
            Title = request.Title,
            Id = Guid.NewGuid(),
            DateCreateChat = DateTimeOffset.UtcNow,
            IsActive = true,
        };

        await _dbContext.Chats.AddAsync(chat, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return chat.Id;
    }
}
