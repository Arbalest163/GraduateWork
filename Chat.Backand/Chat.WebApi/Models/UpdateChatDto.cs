using AutoMapper;
using Chat.Application.Chats.Commands.UpdateChat;
using Chat.Application.Common.Mappings;

namespace Chat.WebApi.Models;

public class UpdateChatDto: IMapWith<UpdateChatCommand>
{
    public Guid ChatId { get; set; } = Guid.Empty;
    public string Title { get; set; } = string.Empty;

    public void Mapping(Profile profile)
    {
        profile.CreateMap<UpdateChatDto, UpdateChatCommand>()
            ;
    }
}
