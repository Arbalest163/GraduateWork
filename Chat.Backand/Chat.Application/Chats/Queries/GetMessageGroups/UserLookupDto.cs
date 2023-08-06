using Chat.Application.Common;
using Chat.Application.Common.Mappings;

namespace Chat.Application.Chats.Queries.GetMessageGroups;

public class UserLookupDto : IMapWith<User>
{
    public Guid Id { get; set; }
    public string Nickname { get; set; }
    public string Avatar { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<User, UserLookupDto>()
            .ForMember(userDto => userDto.Id,
                opt => opt.MapFrom(user => user.Id))
            .ForMember(userDto => userDto.Nickname,
                opt => opt.MapFrom(user => user.Nickname))
            .ForMember(userDto => userDto.Avatar,
                opt => opt.MapFrom(user => Converter.CreateBase64File(user.Avatar)))
        ;
    }
}
