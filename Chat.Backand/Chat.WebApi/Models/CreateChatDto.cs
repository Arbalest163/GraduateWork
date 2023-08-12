using AutoMapper;
using Chat.Application.Chats.Commands.CreateChat;
using Chat.Application.Common;
using Chat.Application.Common.Mappings;

namespace Chat.WebApi.Models;

public class CreateChatDto : IMapWith<CreateChatCommand>
{
    public string Title { get; set; } = string.Empty;
    public string ChatLogo { get; set;} = string.Empty;
    public void Mapping(Profile profile)
    {
        profile.CreateMap<CreateChatDto, CreateChatCommand>()
            .ForMember(chatCommand => chatCommand.Title,
                opt => opt.MapFrom(chatDto => chatDto.Title))
            .ForMember(chatCommand => chatCommand.ChatLogo,
                opt => opt.MapFrom(chatDto => Converter.SaveBase64File(chatDto.ChatLogo)))
            ;
    }
}
