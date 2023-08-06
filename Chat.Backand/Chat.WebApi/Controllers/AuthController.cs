using AutoMapper;
using Chat.Application.Users.Commands.LoginUser;
using Chat.Application.Users.Commands.LogoutUser;
using Chat.Application.Users.Commands.RegisterUser;
using Chat.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Chat.Application.Users.Commands.RefreshTokenUser;
using Microsoft.AspNetCore.Authorization;
using Chat.Application.Users.Queries.GetUser;
using Chat.Application.Users.Commands.EditUser;
using Chat.Application.Users.Queries.GetEditUser;
using Chat.Application.Users.Commands.ChangePassword;

namespace Chat.WebApi.Controllers;

[Route("api/{version:ApiVersion}/auth")]
[Produces("application/json")]
public class AuthController : BaseController
{
    private readonly IMapper _mapper;

    public AuthController(IMapper mapper)
    {
        _mapper = mapper;
    }

    /// <summary>
    /// Вход пользователя
    /// </summary>
    /// <param name="authQuery"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<Token>> Login(AuthQuery authQuery)
    {
        var command = _mapper.Map<LoginUserCommand>(authQuery);
        var token = await Mediator.Send(command);

        return Ok(token);
    }

    /// <summary>
    /// Выход пользователя
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [Route("logout")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Logout()
    {
        var command = new LogoutUserCommand
        {
            UserId = UserPrincipal.UserId.ToString(),
        };
        await Mediator.Send(command);
        return Ok();
    }

    /// <summary>
    /// Обновление токена доступа
    /// </summary>
    /// <param name="refreshTokenCommand">Токен обновления</param>
    /// <returns></returns>
    [HttpPost]
    [Route("refresh-token")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<Token>> RefreshToken([FromBody] RefreshTokenUserCommand refreshTokenCommand)
    {
        var token = await Mediator.Send(refreshTokenCommand);

        return Ok(token);
    }

    /// <summary>
    /// Регистрация пользователя
    /// </summary>
    /// <param name="createUserDto">RegisterUserDto object</param>
    /// <response code="200">Успешно</response>
    [HttpPost]
    [Route("register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Register([FromBody] RegisterUserDto createUserDto)
    {
        var command = _mapper.Map<RegisterUserCommand>(createUserDto);
        await Mediator.Send(command);
        return Ok();
    }

    /// <summary>
    /// Редактирование пользователя
    /// </summary>
    /// <response code="200">Успешно</response>
    [Authorize]
    [HttpGet]
    [Route("edit")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<EditUserVm>> Edit()
    {
        var command = new GetEditUserQuery
        { 
            Id = UserPrincipal.UserId,
        };
        var editUser = await Mediator.Send(command);
        return Ok(editUser);
    }

    /// <summary>
    /// Редактирование пользователя
    /// </summary>
    /// <param name="editUserDto">RegisterUserDto object</param>
    /// <response code="200">Успешно</response>
    [Authorize]
    [HttpPut]
    [Route("edit")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Edit([FromBody] EditUserDto editUserDto)
    {
        var command = _mapper.Map<EditUserCommand>(editUserDto);
        command.UserId = UserPrincipal.UserId;
        await Mediator.Send(command);
        return Ok();
    }

    /// <summary>
    /// Смена пароля
    /// </summary>
    /// <param name="changePasswordDto">RegisterUserDto object</param>
    /// <response code="200">Успешно</response>
    [Authorize]
    [HttpPut]
    [Route("change-password")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Edit([FromBody] ChangePasswordDto changePasswordDto)
    {
        var command = _mapper.Map<ChangePasswordCommand>(changePasswordDto);
        command.UserId = UserPrincipal.UserId;
        await Mediator.Send(command);
        return Ok();
    }

    /// <summary>
    /// Получение текущего авторизованного пользователя
    /// </summary>
    /// <response code="200">Успешно</response>
    [Authorize]
    [HttpGet]
    [Route("get-user")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<UserVm>> GetUser()
    {
        var query = new GetUserQuery
        {
            Id = UserPrincipal.UserId,
        };
        var user = await Mediator.Send(query);
        return Ok(user);
    }

    /// <summary>
    /// Проверка состояния аутентификации пользователя
    /// </summary>
    /// <response code="200">Успешно</response>
    [Authorize]
    [HttpGet]
    [Route("check-state")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<bool> CheckState()
    {
        return Ok(true);
    }
}
