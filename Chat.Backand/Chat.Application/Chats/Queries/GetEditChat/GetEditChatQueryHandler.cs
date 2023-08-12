using AutoMapper.QueryableExtensions;

namespace Chat.Application.Chats.Queries.GetEditChat;

public class GetEditChatQueryHandler
    : IRequestHandler<GetEditChatQuery, EditChatVm>
{
    private readonly IChatDbContext _chatDbContext;
    private readonly IMapper _mapper;

    public GetEditChatQueryHandler(IChatDbContext chatDbContext, IMapper mapper)
    {
        _chatDbContext = chatDbContext;
        _mapper = mapper;
    }

    public async Task<EditChatVm> Handle(GetEditChatQuery request, CancellationToken cancellationToken)
    {
        var chat = await _chatDbContext.Chats
            .Where(c => c.Id == request.ChatId)
            .ProjectTo<EditChatVm>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

        if (chat is null)
        {
            throw new UnauthorizedAccessException();
        }

        return chat;
    }
}
