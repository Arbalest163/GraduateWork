using Microsoft.AspNetCore.Hosting;

namespace Chat.Application.Users.Queries.GetAvatar;

public class GetAvatarQueryHandler
: IRequestHandler<GetAvatarQuery, string>
{
    private readonly IChatDbContext _dbContext;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public GetAvatarQueryHandler(IChatDbContext dbContext,
        IWebHostEnvironment webHostEnvironment) =>
        (_dbContext, _webHostEnvironment) = (dbContext, webHostEnvironment);

    public async Task<string> Handle(GetAvatarQuery request,
        CancellationToken cancellationToken)
    {
        var avatar = await _dbContext.Users
            .Where(user => user.Id == request.UserId)
            .Select(x => new {Path = x.Avatar})
            .FirstOrDefaultAsync(cancellationToken);

        if (avatar == null)
        {
            throw new UnauthorizedAccessException();
        }

        var baseUrl = _webHostEnvironment.WebRootPath;

        string downloadUrl = Path.Combine(baseUrl, avatar.Path);

        return downloadUrl;
    }
}
