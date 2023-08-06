using AutoMapper.QueryableExtensions;
using Chat.Application.Common;

namespace Chat.Application.Chats.Queries.GetChatList;

public class GetChatListQueryHandler
    : IRequestHandler<GetChatListQuery, ChatListVm>
{
    private readonly IChatDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IChatUserPrincipal _currentUser;

    public GetChatListQueryHandler(IChatDbContext dbContext,
        IMapper mapper,
        IChatUserPrincipal currentUser)
    {
        (_dbContext, _mapper, _currentUser) = (dbContext, mapper, currentUser);
    }

    public async Task<ChatListVm> Handle(GetChatListQuery request,
        CancellationToken cancellationToken)
    {
        var filter = request.Filter;
        var query = _dbContext.Chats.Include(x => x.User).AsQueryable();
        if (filter.UserId != Guid.Empty)
        {
            query = query.Where(c => c.User.Id == filter.UserId);
        }

        ApplySearchFilters(ref query, filter.SearchInfo);
        ApplyOrder(ref query, filter.OrderInfo);

        var chats = await query
                .Where(chat => chat.IsActive == true)
                .ProjectTo<ChatLookupDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

        chats.ForEach(c => c.IsCreatorChat = c.UserId == _currentUser.UserId);

        return new ChatListVm { Chats = chats };
    }

    private void ApplySearchFilters(ref IQueryable<Domain.Chat> query, SearchInfo filter)
    {
        var searchText = filter.SearchText;
        var dateCreatedChat = filter.DateCreateChat;

        if (!string.IsNullOrWhiteSpace(searchText))
        {
            query = filter.SearchField switch
            {
                SearchField.Users => query.Where(c => c.User.Nickname.Contains(searchText)),
                SearchField.Messages => query.Where(c => c.Messages.Any(m => m.Text.Contains(searchText))),
                SearchField.Chats => query.Where(c => c.Title.Contains(searchText)),
                _ => query.Where(c => c.User.Nickname.Contains(searchText)),
            };
        }
    }

    private void ApplyOrder(ref IQueryable<Domain.Chat> query, OrderInfo order)
    {
        query = order.Ascending
            ? order.OrderField switch
            {
                OrderField.Date => query.OrderBy(c => c.DateCreateChat),
                OrderField.Title => query.OrderBy(c => c.Title),
                _ => query.OrderBy(c => c.DateCreateChat),
            }
            : order.OrderField switch
            {
                OrderField.Date => query.OrderByDescending(c => c.DateCreateChat),
                OrderField.Title => query.OrderByDescending(c => c.Title),
                _ => query.OrderByDescending(c => c.DateCreateChat),
            };
    }
}
