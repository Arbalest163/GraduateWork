using AutoMapper.QueryableExtensions;

namespace Chat.Application.Users.Queries.GetUser;

public class GetUserQueryHandler : IRequestHandler<GetUserQuery, UserVm>
{
    private readonly IChatDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetUserQueryHandler(IChatDbContext dbContext,
        IMapper mapper) =>
        (_dbContext, _mapper) = (dbContext, mapper);

    public async Task<UserVm> Handle(GetUserQuery request,
        CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users
            .Where(user => user.Id == request.Id)
            .ProjectTo<UserVm>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

        return user ?? throw new UnauthorizedAccessException();
    }
}
