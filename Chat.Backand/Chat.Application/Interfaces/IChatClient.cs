using Chat.Application.Common.Models;

namespace Chat.Application.Interfaces;

public interface IChatClient
{
    Task ReceiveMessage(ReceiveMessage message);
    Task ReceiveInformationMessage(InformationMessage informationMessage);
    Task OnChatCountChange();
}
