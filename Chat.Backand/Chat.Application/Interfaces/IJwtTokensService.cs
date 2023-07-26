namespace Chat.Application.Interfaces;

public interface IJwtTokensService
{
    (string, TimeSpan) GenerateAccessToken(string userId);
     string GenerateRefreshToken(string userId);
    Task DeleteJwtToken(string userId);
    Task ValidateRefreshToken(string refreshToken);
    TokenPayLoad ParseRefreshToken(string refreshToken);
    //Task KeepAliveToken(string token);
    //Task<bool> IfExistToken(string token);
    //Task<int> GetCountActiveToken();
}
