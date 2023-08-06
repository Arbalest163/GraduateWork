using AutoMapper.QueryableExtensions;
using Chat.Application.Common.Exceptions;

namespace Chat.Application.Chats.Queries.GetChat;

public class GetChatQueryHandler
 : IRequestHandler<GetChatQuery, ChatVm>
{
    private readonly IChatDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IChatUserPrincipal _userPrincipal;

    public GetChatQueryHandler(IChatDbContext dbContext,
        IMapper mapper,
        IChatUserPrincipal userPrincipal)
    {
        (_dbContext, _mapper) = (dbContext, mapper);
        _userPrincipal = userPrincipal;
    }

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

        //var user = await _dbContext.Users.Include(x => x.UserRole).FirstAsync(u => u.Id == request.UserId, cancellationToken);

        //chat.Messages = chat.Messages.OrderByDescending(x => x.DateSendMessage).ToList();

        foreach(var message in chat.Messages)
        {
            message.IsCreatorMessage = message.User.Id == _userPrincipal.UserId;
            message.HasRightToEdit = message.IsCreatorMessage || _userPrincipal.Role == Role.Admin;
        }

        chat.GroupMessages = chat.Messages
            .OrderBy(x => x.DateSendMessage)
            .GroupBy(x => x.DateSendMessage, message => message)
            .Select(group => new GroupMessagesDto
            {
                Date = group.Key.ToString("d MMM"),
                Messages = group.ToList()
            })
            .ToList();

        chat.IsCreatorChat = chat.UserId == _userPrincipal.UserId;
        chat.HasRightToEdit = chat.IsCreatorChat || _userPrincipal.Role == Role.Admin;

        return chat;
    }
}
