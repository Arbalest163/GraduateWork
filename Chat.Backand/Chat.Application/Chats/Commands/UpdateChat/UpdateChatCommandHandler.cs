using Chat.Application.Common.Exceptions;

namespace Chat.Application.Chats.Commands.UpdateChat;

public class UpdateChatCommandHandler
    : IRequestHandler<UpdateChatCommand>
{
    private readonly IChatDbContext _dbContext;

    public UpdateChatCommandHandler(IChatDbContext dbContext) =>
        _dbContext = dbContext;

    public async Task Handle(UpdateChatCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users.FirstAsync(u => u.Id == request.UserId);
        if (user == null)
        {
            throw new UnauthorizedAccessException();
        }
        var chat = await _dbContext.Chats
            .Where(chat => chat.Id == request.ChatId)
            .Where(chat => chat.User.Id == user.Id || user.UserRole.Role == Role.Admin)
            .FirstAsync(cancellationToken);

        chat.Title = request.Title;

        if(request.AddUserId is not null)
        {
            var addUser = await _dbContext.Users.FirstAsync(u => u.Id == request.AddUserId);
            chat.Users.Add(addUser);
        }
         if(request.DeleteUserId is not null)
        {
            chat.Users.RemoveAll(user => user.Id == request.UserId);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
