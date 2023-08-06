using Chat.Application.Common;
using Chat.Application.Common.Mappings;

namespace Chat.Application.Chats.Queries.GetChat;

public class ChatUserDto : IMapWith<User>
{
    public Guid Id { get; set; }
    public string Nickname { get; set; }
    public string Avatar { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<User, ChatUserDto>()
            .ForMember(userDto => userDto.Id,
                opt => opt.MapFrom(user => user.Id))
            .ForMember(userDto => userDto.Nickname,
                opt => opt.MapFrom(user => user.Nickname))
            .ForMember(userDto => userDto.Avatar,
                opt => opt.MapFrom(user => Converter.CreateBase64File(user.Avatar)))
        ;
    }
}
