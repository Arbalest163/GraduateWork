using Chat.Application.Common.Mappings;

namespace Chat.Application.Chats.Queries.GetChat;

public class ChatUserDto : IMapWith<User>
{
    public string Nickname { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<User, ChatUserDto>()
            .ForMember(userDto => userDto.Nickname,
                opt => opt.MapFrom(user => user.Nickname))
        ;
    }
}
