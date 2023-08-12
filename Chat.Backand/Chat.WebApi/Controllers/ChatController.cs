using AutoMapper;
using Chat.Application.Chats.Commands.CreateChat;
using Chat.Application.Chats.Commands.CreateMessage;
using Chat.Application.Chats.Commands.DeleteCommand;
using Chat.Application.Chats.Commands.DeleteMessage;
using Chat.Application.Chats.Commands.UpdateChat;
using Chat.Application.Chats.Queries.GetChat;
using Chat.Application.Chats.Queries.GetChatInfo;
using Chat.Application.Chats.Queries.GetChatList;
using Chat.Application.Chats.Queries.GetEditChat;
using Chat.Application.Chats.Queries.GetMessageGroups;
using Chat.Application.Common;
using Chat.Application.Users.Queries.GetAvatar;
using Chat.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Chat.WebApi.Controllers;

[Route("api/{version:ApiVersion}")]
public class ChatController : BaseController
{
    private readonly IMapper _mapper;

    public ChatController(IMapper mapper)
    {
        _mapper = mapper;
    }

    /// <summary>
    /// Получить список чатов
    /// </summary>
    /// <remarks>
    /// Пример запроса:
    /// GET /chats
    /// </remarks>
    /// <returns>Returns ChatListVm</returns>
    /// <response code="200">Успешно</response>
    /// <response code="401">Пользователь не авторизован</response>
    [HttpGet]
    [Authorize]
    [Route("chat/list")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ChatListVm>> GetChats([FromQuery]FilterContext filter)
    {
        var query = new GetChatListQuery
        {
            Filter = filter
        };
        var vm = await Mediator.Send(query);
        return Ok(vm);
    }

    /// <summary>
    /// Получить чат
    /// </summary>
    /// <remarks>
    /// Пример запроса:
    /// GET /chat?ChatId=D34D349E-43B8-429E-BCA4-793C932FD580
    /// </remarks>
    /// <returns>Returns ChatVm</returns>
    /// <response code="200">Успешно</response>
    /// <response code="401">Пользователь не авторизован</response>
    [HttpGet]
    [Authorize]
    [Route("chat")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ChatVm>> GetChat([FromQuery][Required] Guid ChatId)
    {
        var query = new GetChatQuery 
        { 
            ChatId = ChatId,
        };
        var vm = await Mediator.Send(query);
        return Ok(vm);
    }

    /// <summary>
    /// Получить группы сообщений чата
    /// </summary>
    /// <remarks>
    /// Пример запроса:
    /// GET /chat/message-groups?ChatId=D34D349E-43B8-429E-BCA4-793C932FD580
    /// </remarks>
    /// <returns>Returns MessageGroupsVm</returns>
    /// <response code="200">Успешно</response>
    /// <response code="401">Пользователь не авторизован</response>
    [HttpGet]
    [Authorize]
    [Route("chat/message-groups")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<MessageGroupsVm>> GetMessageGroups([FromQuery][Required] Guid ChatId)
    {
        var query = new GetMessageGroupsQuery
        {
            ChatId = ChatId,
        };
        var vm = await Mediator.Send(query);
        return Ok(vm);
    }

    /// <summary>
    /// Создать чат
    /// </summary>
    /// <remarks>
    /// Пример запроса:
    /// POST /chat
    /// {
    ///     title: "chat title"
    /// }
    /// </remarks>
    /// /// <param name="createChatDto">CreateChatDto object</param>
    /// <returns>Returns chat id(Guid)</returns>
    /// <response code="200">Успешно</response>
    /// <response code="401">Пользователь не авторизован</response>
    [HttpPost]
    [Authorize]
    [Route("chat")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<Guid>> CreateChat([FromBody] CreateChatDto createChatDto)
    {
        var command = _mapper.Map<CreateChatCommand>(createChatDto);
        var chatId = await Mediator.Send(command);
        return Ok(chatId);
    }

    [HttpGet]
    [Authorize]
    [Route("chat/edit")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<EditChatVm>> GetEditChat([FromQuery] Guid chatId)
    {
        var query = new GetEditChatQuery
        {
            ChatId = chatId,
        };
        await Mediator.Send(query);
        return NoContent();
    }

    

    /// <summary>
    /// Изменить чат
    /// </summary>
    /// <remarks>
    /// Пример запроса:
    /// PUT /chat
    /// {
    ///     chatid: "chat id",
    ///     title: "chat title"
    /// }
    /// </remarks>
    /// <returns>Returns NoContent</returns>
    /// <response code="204">Успешно</response>
    /// <response code="401">Пользователь не авторизован</response>
    [HttpPut]
    [Authorize]
    [Route("chat")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> UpdateChat([FromBody] UpdateChatDto updateChatDto)
    {
        var command = _mapper.Map<UpdateChatCommand>(updateChatDto);
        await Mediator.Send(command);
        return NoContent();
    }

    /// <summary>
    /// Удалить чат
    /// </summary>
    /// <remarks>
    /// Пример запроса:
    /// DELETE /chat?ChatId=D34D349E-43B8-429E-BCA4-793C932FD580
    /// </remarks>
    /// <returns>Returns NoContent</returns>
    /// <response code="204">Успешно</response>
    /// <response code="401">Пользователь не авторизован</response>
    /// <response code="404">Чат не найден</response>
    [HttpDelete]
    [Authorize]
    [Route("chat")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteChat([Required] Guid ChatId)
    {
        var command = new DeleteChatCommand
        {
            ChatId = ChatId 
        };
        await Mediator.Send(command);
        return NoContent();
    }

    /// <summary>
    /// Написать сообщение в чат
    /// </summary>
    /// <remarks>
    /// Пример запроса:
    /// POST /chat/message
    /// {
    ///     chatid: "chat id",
    ///     message: "text"
    /// }
    /// </remarks>
    /// /// <param name="createMessageDto">CreateMessageDto object</param>
    /// <returns>Returns message(string)</returns>
    /// <response code="200">Успешно</response>
    /// <response code="401">Пользователь не авторизован</response>
    [HttpPost]
    [Authorize]
    [Route("chat/message")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<string>> CreateMessage([FromBody] CreateMessageDto createMessageDto)
    {
        var command = _mapper.Map<CreateMessageCommand>(createMessageDto);
        var message = await Mediator.Send(command);
        return Ok(message);
    }

    /// <summary>
    /// Удалить сообщение
    /// </summary>
    /// <param name="messageId"></param>
    /// <returns></returns>
    [HttpDelete]
    [Authorize]
    [Route("chat/message")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> DeleteMessage([FromQuery] Guid messageId)
    {
        var command = new DeleteMessageCommand
        {
            MessageId = messageId,
        };
        await Mediator.Send(command);
        return Ok();
    }

    /// <summary>
    /// Загрузить файл на сервер
    /// </summary>
    /// <param name="file">UploadFileDto object</param>
    /// <returns>Returns message(string)</returns>
    /// <response code="200">Успешно</response>
    /// <response code="401">Пользователь не авторизован</response>
    [HttpPost]
    [Authorize]
    [Route("chat/upload-file")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<string>> UploadFile([FromForm] UploadFileDto file)
    {
        return Ok();
    }

    /// <summary>
    /// Загрузить аватар с сервера
    /// </summary>
    /// <returns>Returns url(string)</returns>
    /// <response code="200">Успешно</response>
    /// <response code="401">Пользователь не авторизован</response>
    [HttpGet]
    [Authorize]
    [Route("chat/load-avatar")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<string>> LoadAvatar()
    {
        var query = new GetAvatarQuery
        {
            UserId = UserPrincipal.UserId,
        };
        var url = await Mediator.Send(query);
        return Ok(url);
    }

    /// <summary>
    /// Загрузить информаию о чате
    /// </summary>
    /// <returns>Returns ChatInfo</returns>
    /// <response code="200">Успешно</response>
    /// <response code="401">Пользователь не авторизован</response>
    [HttpGet]
    [Authorize]
    [Route("chat/info")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ChatInfoVm>> GetChatInfo([FromQuery] Guid chatId)
    {
        var query = new GetChatInfoQuery
        {
            ChatId = chatId,
        };
        var chatInfo = await Mediator.Send(query);
        return Ok(chatInfo);
    }
}