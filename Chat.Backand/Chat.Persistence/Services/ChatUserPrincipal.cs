
using Chat.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Chat.Persistence.Services;

public class ChatUserPrincipal : IChatUserPrincipal
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ChatUserPrincipal(IHttpContextAccessor httpContextAccessor) =>
        _httpContextAccessor = httpContextAccessor;

    public Guid UserId
    {
        get
        {
            var id = _httpContextAccessor.HttpContext?.User?
               .FindFirstValue(CustomClaimTypes.UserId);
            //var id = "0875ab1b-5078-4432-bffc-e53936f87608";
            Guid.TryParse(id, out var userId);
            return userId;
        }
    }

    public string NickName => _httpContextAccessor.HttpContext?.User?.FindFirstValue(CustomClaimTypes.NickName) ?? string.Empty;

    public Domain.Role Role
    {
        get
        {
            var roleRaw = _httpContextAccessor.HttpContext?.User?
               .FindFirstValue(ClaimTypes.Role);
            Enum.TryParse(roleRaw, true, out Domain.Role role);
            return role;
        }
    }
}
