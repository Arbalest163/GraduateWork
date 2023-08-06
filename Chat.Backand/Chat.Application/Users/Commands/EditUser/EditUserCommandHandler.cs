namespace Chat.Application.Users.Commands.EditUser;

public class EditUserCommandHandler
 : IRequestHandler<EditUserCommand>
{
    private readonly IChatDbContext _chatDbContext;

    public EditUserCommandHandler(IChatDbContext chatDbContext)
    {
        _chatDbContext = chatDbContext;
    }

    public async Task Handle(EditUserCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var user = await _chatDbContext.Users.FirstOrDefaultAsync(x => x.Id == request.UserId);
            if (user == null)
            {
                
                throw new UnauthorizedAccessException();
            }

            user.Firstname = request.Firstname;
            user.Lastname = request.Lastname;
            user.Middlename = request.Middlename;
            user.Birthday = request.Birthday;
            user.Avatar = request.Avatar;
            
            await _chatDbContext.SaveChangesAsync(cancellationToken);

        }
        catch (Exception ex)
        {
            throw new Exception($"Ошибка редактирования. {ex.Message}", ex);
        }
    }
}
