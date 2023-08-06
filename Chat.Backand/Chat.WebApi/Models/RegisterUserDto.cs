using AutoMapper;
using Chat.Application.Common.Mappings;
using Chat.Application.Users.Commands.RegisterUser;
using System.ComponentModel.DataAnnotations;

namespace Chat.WebApi.Models;

public class RegisterUserDto : IMapWith<RegisterUserCommand>
{
    public string Username { get; set; } = string.Empty;
    public string Nickname { get; set; } = string.Empty;

    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
    [DataType(DataType.Password)]
    public string ConfirmPassword { get; set; } = string.Empty;

    public void Mapping(Profile profile)
    {
        profile.CreateMap<RegisterUserDto, RegisterUserCommand>()
            ;
    }
}
