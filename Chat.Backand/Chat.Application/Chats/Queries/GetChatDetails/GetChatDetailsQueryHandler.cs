using AutoMapper.QueryableExtensions;

namespace Chat.Application.Chats.Queries.GetChatDetails;

public class GetChatDetailsQueryHandler
    : IRequestHandler<GetChatDetailsQuery, ChatDetailsVm>
{
    private readonly IChatDbContext _chatDbContext;
    private readonly IMapper _mapper;

    public GetChatDetailsQueryHandler(IChatDbContext chatDbContext, IMapper mapper)
    {
        _chatDbContext = chatDbContext;
        _mapper = mapper;
    }

    public async Task<ChatDetailsVm> Handle(GetChatDetailsQuery request, CancellationToken cancellationToken)
    {
        var chatVm = await _chatDbContext.Chats
            .Where(x => x.Id == request.ChatId)
            .ProjectTo<ChatDetailsVm>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);
        return chatVm;
    }
}
