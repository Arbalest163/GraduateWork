namespace Chat.Application.Interfaces;

public interface IPasswordManager
{
    Task<bool> CheckPasswords(string password, string hashPassword);
    Task<string> GetHashPassword(string password);
}
