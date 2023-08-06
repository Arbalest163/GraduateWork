namespace Chat.Application.Users.Queries.GetAvatar;

public class GetAvatarQuery : IRequest<string>
{
    public Guid UserId { get; set; }
}
