using Chat.Application.Common;
using Chat.Application.Common.Mappings;

namespace Chat.Application.Users.Queries.GetEditUser;

public class EditUserVm : IMapWith<User>
{
    public string Avatar { get; set; } = string.Empty;
    public string Firstname { get; set; } = string.Empty;
    public string Lastname { get; set; } = string.Empty;
    public string Middlename { get; set; } = string.Empty;
    public DateTimeOffset? Birthday { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<User, EditUserVm>()
            .ForMember(userDto => userDto.Avatar,
                opt => opt.MapFrom(user => Converter.CreateBase64File(user.Avatar)))
            ;
    }
}
