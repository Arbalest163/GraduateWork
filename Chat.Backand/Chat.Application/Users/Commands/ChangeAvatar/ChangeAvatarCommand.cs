namespace Chat.Application.Users.Commands.ChangeAvatar;

public class ChangeAvatarCommand : IRequest
{
    public Guid UserId { get; set; }
    public byte[] AvatarData { get; set; }
}
