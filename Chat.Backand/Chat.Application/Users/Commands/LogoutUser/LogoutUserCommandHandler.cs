namespace Chat.Application.Users.Commands.LogoutUser;

public class LogoutUserCommandHandler
    : IRequestHandler<LogoutUserCommand>
{
    private readonly IJwtTokensService _jwtTokensService;
    public LogoutUserCommandHandler(IJwtTokensService jwtTokensService)
    {
        _jwtTokensService = jwtTokensService;
    }

    public async Task Handle(LogoutUserCommand request, CancellationToken cancellationToken)
    {
        await _jwtTokensService.DeleteJwtToken(request.UserId);
    }
}
