using AutoMapper;
using Chat.Application.Chats.Commands.CreateMessage;
using Chat.Application.Common.Mappings;

namespace Chat.WebApi.Models;

public class CreateMessageDto : IMapWith<CreateMessageCommand>
{
    public Guid ChatId { get; set; } = Guid.Empty;
    public string Message { get; set; } = string.Empty;

    public void Mapping(Profile profile)
    {
        profile.CreateMap<CreateMessageDto, CreateMessageCommand>();
    }
}
