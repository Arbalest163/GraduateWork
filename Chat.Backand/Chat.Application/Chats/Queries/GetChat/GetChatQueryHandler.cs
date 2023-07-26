using AutoMapper.QueryableExtensions;
using Chat.Application.Common.Exceptions;

namespace Chat.Application.Chats.Queries.GetChat;

public class GetChatQueryHandler
 : IRequestHandler<GetChatQuery, ChatVm>
{
    private readonly IChatDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetChatQueryHandler(IChatDbContext dbContext,
        IMapper mapper) =>
        (_dbContext, _mapper) = (dbContext, mapper);

    public async Task<ChatVm> Handle(GetChatQuery request,
        CancellationToken cancellationToken)
    {
        var chat = await _dbContext.Chats
            .Where(chat => chat.Id == request.ChatId)
            .ProjectTo<ChatVm>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

        if(chat == null)
        {
            throw new NotFoundException(nameof(Domain.Chat), request.ChatId);
        }

        chat.Messages = chat.Messages.OrderByDescending(x => x.DateSendMessage).ToList();

        return chat;
    }
}
