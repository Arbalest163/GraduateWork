using Chat.Application.Common;

namespace Chat.Application.Chats.Queries.GetChatList;

public class GetChatListQuery : IRequest<ChatListVm>
{
    public FilterContext Filter { get; set; } = new FilterContext();
}
