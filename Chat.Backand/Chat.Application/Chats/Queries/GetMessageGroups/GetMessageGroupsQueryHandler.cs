using AutoMapper.QueryableExtensions;

namespace Chat.Application.Chats.Queries.GetMessageGroups;

public class GetMessageGroupsQueryHandler
  : IRequestHandler<GetMessageGroupsQuery, MessageGroupsVm>
{
    private readonly IChatDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IChatUserPrincipal _userPrincipal;

    public GetMessageGroupsQueryHandler(IChatDbContext dbContext, IMapper mapper, IChatUserPrincipal userPrincipal)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _userPrincipal = userPrincipal;
    }

    public async Task<MessageGroupsVm> Handle(GetMessageGroupsQuery request, CancellationToken cancellationToken)
    {
        var messages = await _dbContext.Messages
            .Where(m => m.Chat.Id == request.ChatId)
            .ProjectTo<MessageLookupDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        messages.ForEach(m =>
        {
            m.IsCreatorMessage = m.User.Id == _userPrincipal.UserId;
            m.HasRightToEdit = m.IsCreatorMessage || _userPrincipal.Role == Role.Admin;
        });

        var messageGroups = messages
            .OrderBy(x => x.DateSend)
            .GroupBy(x => x.DateSend, m => m)
            .Select(group => new MessageGroupDto
            {
                Date = group.Key.ToString("d MMM"),
                Messages = group.OrderBy(x => x.TimeSend).ToList(),
            })
            .ToList();

        return new MessageGroupsVm 
        { 
            MessageGroups = messageGroups
        };
    }
}
