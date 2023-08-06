using AutoMapper.QueryableExtensions;
using Chat.Application.Common.Exceptions;

namespace Chat.Application.Chats.Queries.GetChatInfo;

public class GetChatInfoQueryHandler
    : IRequestHandler<GetChatInfoQuery, ChatInfoVm>
{
    private readonly IChatDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IChatUserPrincipal _userPrincipal;

    public GetChatInfoQueryHandler(IChatDbContext dbContext, IMapper mapper, IChatUserPrincipal userPrincipal)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _userPrincipal = userPrincipal;
    }

    public async Task<ChatInfoVm> Handle(GetChatInfoQuery request, CancellationToken cancellationToken)
    {
        var chatVm = await _dbContext.Chats
            .Where(c => c.Id == request.ChatId)
            .ProjectTo<ChatInfoVm>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);
        if (chatVm == null)
        {
            throw new NotFoundException("Чат не найден!");
        }

        chatVm.HasRightToEdit = chatVm.UserId == _userPrincipal.UserId || _userPrincipal.Role == Role.Admin;
        return chatVm;
    }
}
