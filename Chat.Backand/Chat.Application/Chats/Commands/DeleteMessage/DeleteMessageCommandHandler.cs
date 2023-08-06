namespace Chat.Application.Chats.Commands.DeleteMessage;

public class DeleteMessageCommandHandler
    : IRequestHandler<DeleteMessageCommand>
{
    private readonly IChatDbContext _chatDbContext;
    private readonly IChatUserPrincipal _userPrincipal;

    public DeleteMessageCommandHandler(IChatDbContext chatDbContext, IChatUserPrincipal userPrincipal)
    {
        _chatDbContext = chatDbContext;
        _userPrincipal = userPrincipal;
    }

    public async Task Handle(DeleteMessageCommand request, CancellationToken cancellationToken)
    {
        var message = await _chatDbContext.Messages
                .FirstOrDefaultAsync(x => x.Id == request.MessageId);

        if(message.User.Id == _userPrincipal.UserId || _userPrincipal.Role == Role.Admin)
        {
            _chatDbContext.Messages.Remove(message);
            await _chatDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
