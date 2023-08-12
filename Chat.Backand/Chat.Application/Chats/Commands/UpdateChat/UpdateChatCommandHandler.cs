namespace Chat.Application.Chats.Commands.UpdateChat;

public class UpdateChatCommandHandler
    : IRequestHandler<UpdateChatCommand>
{
    private readonly IChatDbContext _dbContext;
    private readonly IChatUserPrincipal _userPrincipal;

    public UpdateChatCommandHandler(IChatDbContext dbContext, IChatUserPrincipal userPrincipal)
    {
        _dbContext = dbContext;
        _userPrincipal = userPrincipal;
    }

    public async Task Handle(UpdateChatCommand request,
        CancellationToken cancellationToken)
    {
        var chat = await _dbContext.Chats
            .Include(c => c.User)
            .Where(chat => chat.Id == request.ChatId)
            .FirstAsync(cancellationToken);

        if(chat.User.Id == _userPrincipal.UserId || _userPrincipal.Role == Role.Admin)
        {
            chat.Title = request.Title;
            chat.ChatLogo = request.ChatLogo;

            if (request.AddUserId is not null)
            {
                var addUser = await _dbContext.Users.FirstAsync(u => u.Id == request.AddUserId);
                chat.Users.Add(addUser);
            }
            if (request.DeleteUserId is not null)
            {
                chat.Users.RemoveAll(user => user.Id == request.DeleteUserId);
            }

            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
