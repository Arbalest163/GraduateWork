using AutoMapper.QueryableExtensions;

namespace Chat.Application.Chats.Queries.GetChatList;

public class GetChatListQueryHandler
    : IRequestHandler<GetChatListQuery, ChatListVm>
{
    private readonly IChatDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetChatListQueryHandler(IChatDbContext dbContext,
        IMapper mapper) =>
        (_dbContext, _mapper) = (dbContext, mapper);

    public async Task<ChatListVm> Handle(GetChatListQuery request,
        CancellationToken cancellationToken)
    {
        var query = _dbContext.Chats.AsQueryable();
        if (request.UserId != Guid.Empty)
        {
            query = query.Where(c => c.User.Id == request.UserId);
        }

        ApplySearchFilters(ref query, request.SearchInfo);
        ApplyOrder(ref query, request.OrderInfo);

        var chats = await query
                .Where(chat => chat.IsActive == true)
                .ProjectTo<ChatLookupDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

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
                SearchField.Title => query.Where(c => c.Title.Contains(searchText)),
                SearchField.DateCreateChat => dateCreatedChat != null ? query.Where(c => c.DateCreateChat == dateCreatedChat) : query,
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
