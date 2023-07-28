using Microsoft.IdentityModel.Tokens;
using Chat.Persistence.Services;

namespace Chat.Persistence;

public class Configuration
{
    private static Configuration? _instance;
    public static Configuration Instance => _instance ??= new Configuration();

    private readonly Lazy<TokenValidationParameters> _validateOptions = new Lazy<TokenValidationParameters>(InitValidationParameters);
    public TokenValidationParameters TokenValidationParameters => _validateOptions.Value;

    private static TokenValidationParameters InitValidationParameters()
    {
        return new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = "ChatWebApi",
            ValidateLifetime = true,
            IssuerSigningKey = SigningCertificate.Get().Key,
            ValidateIssuerSigningKey = true,
            ValidAudience = "ChatWebApi",
            ValidateAudience = true,
            RequireExpirationTime = true,
            ClockSkew = TimeSpan.Zero,
        };
    }
}
