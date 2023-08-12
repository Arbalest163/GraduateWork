using Chat.Application.Common;
using Chat.Application.Common.Mappings;

namespace Chat.Application.Users.Queries.GetUser;

public class UserVm : IMapWith<User>
{
    public Guid Id { get; set; }
    public string Nickname { get; set; }
    public string Avatar { get; set; }
    public Guid[] MainChats { get; set; }
    public string Role { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<User, UserVm>()
            .ForMember(userDto => userDto.Id,
                opt => opt.MapFrom(user => user.Id))
            .ForMember(userDto => userDto.Nickname,
                opt => opt.MapFrom(user => user.Nickname))
            .ForMember(userDto => userDto.Avatar,
                opt => opt.MapFrom(user => Converter.CreateBase64File(user.Avatar)))
            .ForMember(userDto => userDto.MainChats,
                opt => opt.MapFrom(user => user.UserChats.Select(x => x.Id)))
            .ForMember(userDto => userDto.Role,
                opt => opt.MapFrom(user => user.UserRole.Role.ToString()))
            ;
    }
}