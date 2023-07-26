namespace Chat.Application.Users.Queries.GetUser;

public class GetUserQuery : IRequest<UserVm>
{
    public Guid Id { get; set; }
}
