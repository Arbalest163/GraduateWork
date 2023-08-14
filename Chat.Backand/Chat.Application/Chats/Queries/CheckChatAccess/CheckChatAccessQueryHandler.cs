namespace Chat.Application.Chats.Queries.CheckChatAccess;

public class CheckChatAccessQueryHandler
    : IRequestHandler<CheckChatAccessQuery, bool>
{
    private readonly IChatDbContext _chatDbContext;
    private readonly IChatUserPrincipal _userPrincipal;

    public CheckChatAccessQueryHandler(IChatDbContext chatDbContext, IChatUserPrincipal userPrincipal)
    {
        _chatDbContext = chatDbContext;
        _userPrincipal = userPrincipal;
    }

    public async Task<bool> Handle(CheckChatAccessQuery request, CancellationToken cancellationToken)
    {
        var chat = await _chatDbContext.Chats
            .Where(c => c.Id == request.ChatId)
            .Where(c => c.User.Id == _userPrincipal.UserId || _userPrincipal.Role == Role.Admin || c.Users.Any(x => x.Id == _userPrincipal.UserId))
            .FirstOrDefaultAsync(cancellationToken);

        return chat is not null ? true : false;
    }
}
