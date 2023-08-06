namespace Chat.Application.Users.Commands.ChangePassword;

public class ChangePasswordCommandHandler
 : IRequestHandler<ChangePasswordCommand>
{
    private readonly IChatDbContext _chatDbContext;
    private readonly IPasswordManager _passwordManager;

    public ChangePasswordCommandHandler(IChatDbContext chatDbContext, IPasswordManager passwordManager)
    {
        _chatDbContext = chatDbContext;
        _passwordManager = passwordManager;
    }

    public async Task Handle(ChangePasswordCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var user = await _chatDbContext.Users.FirstOrDefaultAsync(x => x.Id == request.UserId);
            if (user == null)
            {

                throw new UnauthorizedAccessException();
            }

            var passHash = await _passwordManager.GetHashPassword(request.Password);
            user.PasswordHash = passHash;

            await _chatDbContext.SaveChangesAsync(cancellationToken);

        }
        catch (Exception ex)
        {
            throw new Exception($"Ошибка редактирования. {ex.Message}", ex);
        }
    }
}
