using Chat.Application.Interfaces;
using Chat.Domain;
using IdentityModel;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Chat.Persistence.Services;

public class JwtTokensService : IJwtTokensService
{
    private const string AccessTypeToken = "access_token";
    private const string RefreshTypeToken = "refresh_token";

    private readonly IDatabase _redisDb;
    private readonly ChatApiOptions _options;

    public JwtTokensService(IDatabase redisDb, ChatApiOptions options)
    {
        _redisDb = redisDb;
        _options = options;
    }

    public (string, TimeSpan) GenerateAccessToken(string userId, string role)
    {
        var expiredTime = TimeSpan.FromMinutes(_options.LifeTimeAccessToken);
        var scope = "ChatWebApi";
        var accessToken = GenerateToken(userId, role, expiredTime, scope);

        var tokenHandler = new JwtSecurityTokenHandler();
        string encodedToken = tokenHandler.WriteToken(accessToken);

        var redisKey = GetRedisKey(userId, AccessTypeToken);

        StoreJwtToken(encodedToken, expiredTime, redisKey);

        return (encodedToken, expiredTime);
    }

    public string GenerateRefreshToken(string userId)
    {
        var expiredTime = TimeSpan.FromDays(_options.LifeTimeRefreshToken);
        var refreshToken = GenerateToken(userId, expiredTime);

        var tokenHandler = new JwtSecurityTokenHandler();
        string encodedRefreshToken = tokenHandler.WriteToken(refreshToken);

        var redisKey = GetRedisKey(userId, RefreshTypeToken);

        StoreJwtToken(encodedRefreshToken, expiredTime, redisKey);

        return encodedRefreshToken;
    }

    public async Task DeleteJwtToken(string userId)
    {
        var redisKeyAccess = GetRedisKey(userId, AccessTypeToken);
        var redisKeyRefresh = GetRedisKey(userId, RefreshTypeToken);
        await _redisDb.KeyDeleteAsync(redisKeyAccess);
        await _redisDb.KeyDeleteAsync(redisKeyRefresh);
    }

    public async Task ValidateRefreshToken(string refreshToken)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        tokenHandler.ValidateToken(refreshToken, Configuration.Instance.TokenValidationParameters, out SecurityToken validatedToken);
        JwtSecurityToken jwt = tokenHandler.ReadJwtToken(refreshToken);
        var tokenIsStore = await CheckTokenInStore(jwt, refreshToken);
        if (!tokenIsStore)
        {
            throw new Exception("invalid refresh_token");
        }
    }

    public TokenPayLoad ParseRefreshToken(string refreshToken)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        JwtSecurityToken jwt = tokenHandler.ReadJwtToken(refreshToken);

        return ParseJwt(jwt);
    }

    private Task<bool> StoreJwtToken(string encodedToken, TimeSpan expiration, string storeKey)
    {
        return _redisDb.StringSetAsync(storeKey, encodedToken, expiration);
    }

    private string GetRedisKey(JwtSecurityToken token, string typeToken)
    {
        var payLoad = ParseJwt(token);

        return GetRedisKey(payLoad.UserId.ToString(), typeToken);
    }

    private string GetRedisKey(string userId, string typeToken)
    {
        return RedisKeyHelper.BuildKey("services", "chat_webapi", typeToken, userId);
    }

    private JwtSecurityToken GenerateToken(string userId, TimeSpan ttl, string? scope = null)
    {
        return GenerateToken(userId, null, ttl, scope);
    }
    private JwtSecurityToken GenerateToken(string userId, string role, TimeSpan ttl, string? scope = null)
    {
        DateTime now = DateTime.UtcNow;
        var tokenHandler = new JwtSecurityTokenHandler();

        var claims = new List<Claim>()
        {
            new Claim(CustomClaimTypes.UserId, userId),
            
        };

        if (!string.IsNullOrWhiteSpace(role))
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        if (scope != null)
        {
            claims.Add(new Claim(JwtClaimTypes.Scope, scope));
        }

        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Issuer = "ChatWebApi",
            Subject = new ClaimsIdentity(claims),
            Expires = now.Add(ttl),
            SigningCredentials = SigningCertificate.Get(),
            Audience = "ChatWebApi",
        };

        var token = tokenHandler.CreateToken(tokenDescriptor) as JwtSecurityToken;

        return token ?? throw new Exception("Ошибка создания токена.");
    }

    private async Task<bool> CheckTokenInStore(JwtSecurityToken jwtRefreshToken, string refreshToken)
    {
        string storeKey = GetRedisKey(jwtRefreshToken, RefreshTypeToken);

        var storedRefreshToken = await _redisDb.StringGetAsync(storeKey);
        if (storedRefreshToken.IsNullOrEmpty)
        {
            return false;
        }

        return string.Equals(refreshToken, storedRefreshToken, StringComparison.InvariantCultureIgnoreCase);
    }

    private TokenPayLoad ParseJwt(JwtSecurityToken jwt)
    {
        var userIdRaw = jwt.Claims.FirstOrDefault(c => c.Type == CustomClaimTypes.UserId)?.Value;

        Guid.TryParse(userIdRaw, out var userId);

        var payLoad = new TokenPayLoad
        {
            UserId = userId
        };

        return payLoad;
    }
}

