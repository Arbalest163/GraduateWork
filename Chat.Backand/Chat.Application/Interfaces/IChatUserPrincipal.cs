namespace Chat.Application.Interfaces;

public interface IChatUserPrincipal
{
    Guid UserId { get; }
    string NickName { get; }
    string Role { get; }
}
