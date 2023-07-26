using Chat.Application.Common.Exceptions;

namespace Chat.Application.Users.Commands.LoginUser;

public class LoginUserCommandHandler
: IRequestHandler<LoginUserCommand, Token>
{
    private readonly IChatDbContext _chatDbContext;
    private readonly IPasswordManager _passwordManager;
    private readonly IJwtTokensService _jwtTokensService;

    public LoginUserCommandHandler(IChatDbContext chatDbContext, IPasswordManager passwordManager, IJwtTokensService jwtTokensService)
    {
        _chatDbContext = chatDbContext;
        _passwordManager = passwordManager;
        _jwtTokensService = jwtTokensService;
    }

    public async Task<Token> Handle(LoginUserCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _chatDbContext.Users.FirstOrDefaultAsync(u => u.UserName == request.Login);
        if (user is null)
        {
            throw new NotFoundException($"Пользователь с логином {request.Login} не найден!");
        }

        var result = await _passwordManager.CheckPasswords(request.Password, user.PasswordHash);
        if (result is false)
        {
            throw new UnauthorizedAccessException("Не правильный логин или пароль!");
        }

        var (accessToken, expireTime) = _jwtTokensService.GenerateAccessToken(user.Id.ToString());
        var refreshToken = _jwtTokensService.GenerateRefreshToken(user.Id.ToString());

        var token = new Token
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            Expires = (int)expireTime.TotalSeconds,
        };

        return token;
    }
}
