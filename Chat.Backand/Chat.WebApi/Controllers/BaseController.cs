using Chat.Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Chat.WebApi.Controllers;

[ApiController]
public abstract class BaseController : ControllerBase
{
    private IMediator _mediator;
    private IChatUserPrincipal _userPrincipal;
    protected IMediator Mediator =>
        _mediator ??= HttpContext.RequestServices.GetRequiredService<IMediator>();

    protected IChatUserPrincipal UserPrincipal => 
        _userPrincipal ??= HttpContext.RequestServices.GetRequiredService<IChatUserPrincipal>();

}
