using AutoMapper;
using Chat.Application.Common.Mappings;
using Chat.Application.Users.Commands.ChangePassword;
using System.ComponentModel.DataAnnotations;

namespace Chat.WebApi.Models;

public class ChangePasswordDto : IMapWith<ChangePasswordCommand>
{
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
    [DataType(DataType.Password)]
    public string ConfirmPassword { get; set; } = string.Empty;

    public void Mapping(Profile profile)
    {
        profile.CreateMap<ChangePasswordDto, ChangePasswordCommand>()
            ;
    }
}
