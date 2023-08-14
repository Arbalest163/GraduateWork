using Chat.Application.Common;
using Chat.Application.Common.Mappings;

namespace Chat.Application.Chats.Queries.GetChatDetails;

public class ChatMemberDto : IMapWith<User>
{
    public string Nickname { get; set; }
    public string Avatar { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<User, ChatMemberDto>()
            .ForMember(userDto => userDto.Nickname,
                opt => opt.MapFrom(user => user.Nickname))
            .ForMember(userDto => userDto.Avatar,
                opt => opt.MapFrom(user => Converter.CreateBase64File(user.Avatar)))
        ;
    }
}
