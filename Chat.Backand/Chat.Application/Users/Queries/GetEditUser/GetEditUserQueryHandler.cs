using AutoMapper.QueryableExtensions;

namespace Chat.Application.Users.Queries.GetEditUser;

public class GetEditUserQueryHandler
: IRequestHandler<GetEditUserQuery, EditUserVm>
{
    private readonly IChatDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetEditUserQueryHandler(IChatDbContext dbContext,
        IMapper mapper)
    {
        (_dbContext, _mapper) = (dbContext, mapper);
    }

    public async Task<EditUserVm> Handle(GetEditUserQuery request,
        CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users
            .Where(user => user.Id == request.Id)
            .ProjectTo<EditUserVm>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

        if (user == null)
        {
            throw new UnauthorizedAccessException();
        }

        return user;
    }
}
