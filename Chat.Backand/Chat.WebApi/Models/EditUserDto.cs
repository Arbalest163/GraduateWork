using AutoMapper;
using Chat.Application.Common;
using Chat.Application.Common.Mappings;
using Chat.Application.Users.Commands.EditUser;
using System.ComponentModel.DataAnnotations;

namespace Chat.WebApi.Models;

public class EditUserDto : IMapWith<EditUserCommand>
{
    public string Avatar { get; set; } = string.Empty;
    public string Firstname { get; set; } = string.Empty;
    public string Lastname { get; set; } = string.Empty;
    public string Middlename { get; set; } = string.Empty;

    [DataType(DataType.Date)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "dd.MM.yyyy")]
    public DateTimeOffset? Birthday { get; set; }

    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
    [DataType(DataType.Password)]
    public string ConfirmPassword { get; set; } = string.Empty;

    public void Mapping(Profile profile)
    {
        profile.CreateMap<EditUserDto, EditUserCommand>()
            .ForMember(userDto => userDto.Avatar,
                opt => opt.MapFrom(user => Converter.SaveBase64File(user.Avatar)))
            ;
    }
}
