using Chat.Application.Users.Commands.LoginUser;

namespace Chat.Application.Users.Commands.RefreshTokenUser
{
    public class RefreshTokenUserCommandHandler
    : IRequestHandler<RefreshTokenUserCommand, Token>
    {
        private readonly IChatDbContext _chatDbContext;
        private readonly IJwtTokensService _jwtTokensService;

        public RefreshTokenUserCommandHandler(IChatDbContext chatDbContext, IJwtTokensService jwtTokensService)
        {
            _chatDbContext = chatDbContext;
            _jwtTokensService = jwtTokensService;
        }

        public async Task<Token> Handle(RefreshTokenUserCommand request,
            CancellationToken cancellationToken)
        {
            await _jwtTokensService.ValidateRefreshToken(request.RefreshToken);

            var tokenPayload = _jwtTokensService.ParseRefreshToken(request.RefreshToken);

            var user = await _chatDbContext.Users.FirstOrDefaultAsync(u => u.Id == tokenPayload.UserId);
            if (user is null)
            {
                throw new UnauthorizedAccessException();
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
}
