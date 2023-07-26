using Chat.Application.Common.Mappings;

namespace Chat.Application.Chats.Queries.GetChatList;

public class UserLookupDto : IMapWith<User>
{
    public string Nickname { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<User, UserLookupDto>()
            .ForMember(userDto => userDto.Nickname,
                opt => opt.MapFrom(user => user.Nickname))
        ;
    }
}
