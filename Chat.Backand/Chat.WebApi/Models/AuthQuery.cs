using AutoMapper;
using Chat.Application.Common.Mappings;
using Chat.Application.Users.Commands.LoginUser;

namespace Chat.WebApi.Models;

public class AuthQuery : IMapWith<LoginUserCommand>
{
    public string Login { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    public void Mapping(Profile profile)
    {
        profile.CreateMap<AuthQuery, LoginUserCommand>()
            ;
    }
}
