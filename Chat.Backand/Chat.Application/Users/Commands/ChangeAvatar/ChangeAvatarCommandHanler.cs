using Chat.Application.Common;

namespace Chat.Application.Users.Commands.ChangeAvatar
{
    public class ChangeAvatarCommandHanler
        : IRequestHandler<ChangeAvatarCommand>
    {
        private readonly IChatDbContext _chatDbContext;

        public ChangeAvatarCommandHanler(IChatDbContext chatDbContext)
        {
            _chatDbContext = chatDbContext;
        }

        public async Task Handle(ChangeAvatarCommand request, CancellationToken cancellationToken)
        {
            var user = await _chatDbContext.Users.FirstOrDefaultAsync(x => x.Id == request.UserId, cancellationToken);

            if (user == null)
            {
                throw new UnauthorizedAccessException();
            }

            if (!request.AvatarData.Any())
            { 

            }

            var fileName = $"{user.Id}_{DateTimeOffset.Now}.jpg";
            var avatarPath = FileHelper.SaveAvatar(fileName, request.AvatarData);
            user.Avatar = avatarPath;
            await _chatDbContext.SaveChangesAsync(cancellationToken);
            
        }
    }
}
