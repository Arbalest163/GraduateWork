namespace Chat.Application.Users.Queries.GetEditUser;

public class GetEditUserQuery : IRequest<EditUserVm>
{
    public Guid Id { get; set; }
}
