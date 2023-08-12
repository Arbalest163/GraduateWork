namespace Chat.Application.Chats.Commands.CreateChat;

public class CreateChatCommandHandler
 : IRequestHandler<CreateChatCommand, Guid>
{
    private readonly IChatDbContext _dbContext;
    private readonly IChatUserPrincipal _userPrincipal;

    public CreateChatCommandHandler(IChatDbContext dbContext, IChatUserPrincipal userPrincipal)
    {
        _dbContext = dbContext;
        _userPrincipal = userPrincipal;
    }

    public async Task<Guid> Handle(CreateChatCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users.FirstAsync(x => x.Id == _userPrincipal.UserId, cancellationToken);
        var chat = new Domain.Chat
        {
            User = user,
            Users = new List<User>() { user },
            Title = request.Title,
            ChatLogo = request.ChatLogo,
            Id = Guid.NewGuid(),
            DateCreateChat = DateTimeOffset.UtcNow,
            IsActive = true,
        };

        await _dbContext.Chats.AddAsync(chat, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return chat.Id;
    }
}
