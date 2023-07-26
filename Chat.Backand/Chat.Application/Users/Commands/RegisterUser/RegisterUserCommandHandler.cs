namespace Chat.Application.Users.Commands.RegisterUser;

public class RegisterUserCommandHandler
 : IRequestHandler<RegisterUserCommand>
{
    private readonly IChatDbContext _chatDbContext;
    private readonly IPasswordManager _passwordManager;

    public RegisterUserCommandHandler(IChatDbContext chatDbContext, IPasswordManager passwordManager)
    {
        _chatDbContext = chatDbContext;
        _passwordManager = passwordManager;
    }

    public async Task Handle(RegisterUserCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            if(request.Password != request.ConfirmPassword)
            {
                throw new Exception("Пароли не совпадают.");
            }
            var existUser = await _chatDbContext.Users.Where(u => u.UserName == request.Username || u.Nickname == request.Nickname).Select(u => new { u.UserName, u.Nickname }).FirstOrDefaultAsync();
            if (existUser != null)
            {
                var filed = existUser.UserName == request.Username ? $"Username {request.Username}" : $"Nickname {request.Nickname}";
                throw new Exception($"Пользователь с таким {filed} уже существует!");
            }

            var passHash = await _passwordManager.GetHashPassword(request.Password);
            var userRole = await _chatDbContext.UserRoles.FirstAsync(x => x.Role == Role.User);

            var user = new User
            {
                Id = Guid.NewGuid(),
                UserName = request.Username.Trim(),
                Firstname = request.Firstname.Trim(),
                Lastname = request.Lastname.Trim(),
                Middlename = request.Middlename.Trim(),
                Birthday = request.Birthday.Value,
                Nickname = request.Nickname.Trim(),
                PasswordHash = passHash,
                UserRole = userRole,
            };

            await _chatDbContext.Users.AddAsync(user, cancellationToken);
            await _chatDbContext.SaveChangesAsync(cancellationToken);

        }
        catch (Exception ex)
        {
            throw new Exception($"Ошибка регистрации. {ex.Message}", ex);
        }
    }
}
