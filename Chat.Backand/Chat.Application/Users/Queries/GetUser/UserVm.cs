using Chat.Application.Common.Mappings;

namespace Chat.Application.Users.Queries.GetUser;

public class UserVm : IMapWith<User>
{
    public string Nickname { get; set; }
    public Guid[] MainChats { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<User, UserVm>()
            .ForMember(userDto => userDto.Nickname,
                opt => opt.MapFrom(user => user.Nickname))
            .ForMember(userDto => userDto.MainChats,
                opt => opt.MapFrom(user => user.UserChats.Select(x => x.Id)))
            ;
    }
}