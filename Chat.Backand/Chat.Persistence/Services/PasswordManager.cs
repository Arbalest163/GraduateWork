using Chat.Application.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace Chat.Persistence.Services;

public class PasswordManager : IPasswordManager
{
    private readonly ChatApiOptions _options;

    public PasswordManager(ChatApiOptions options)
    {
        _options = options;
    }

    public async Task<bool> CheckPasswords(string password, string hashPassword)
    {
        string hashedPassword = await HashPassword(password);
        bool passwordsMatch = hashPassword.Equals(hashedPassword, StringComparison.OrdinalIgnoreCase);

        return passwordsMatch;
    }

    public async Task<string> GetHashPassword(string password)
    {
        string hashedPassword = await HashPassword(password);
        return hashedPassword;
    }

    private async Task<string> HashPassword(string password)
    {
        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_options.ServiceKey));
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(password));
        var hash = await hmac.ComputeHashAsync(stream);
        return Convert.ToBase64String(hash)
            .Replace("+", "-")
            .Replace("/", "_")
            .TrimEnd('=');
    }

}
