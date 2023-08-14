using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Chat.Application.Hubs;

[Authorize]
public class ChatHub : Hub<IChatClient>
{
    private readonly IChatUserPrincipal _userPrincipal;

    public ChatHub(IChatUserPrincipal userPrincipal)
    {
        _userPrincipal = userPrincipal;
    }

    public override async Task OnConnectedAsync()
    {
        //await Clients.All.Test($"{_userPrincipal.NickName} онлайн");
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        //await Clients.All.ReceiveMessage($"{_userPrincipal.NickName} офлайн");
    }

    public async Task JoinChatGroup(string chatId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, chatId);
    }

    public async Task LeaveChatGroup(string chatId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatId);
    }
}
