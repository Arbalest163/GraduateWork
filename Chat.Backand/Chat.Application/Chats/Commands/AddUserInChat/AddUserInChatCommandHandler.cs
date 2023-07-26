using Chat.Application.Common.Exceptions;
using Chat.Domain;

namespace Chat.Application.Chats.Commands.AddUserInChat
{
    public class AddUserInChatCommandHandler
        : IRequestHandler<AddUserInChatCommand>
    {
        private readonly IChatDbContext _dbContext;

        public AddUserInChatCommandHandler(IChatDbContext dbContext) =>
            _dbContext = dbContext;

        public async Task Handle(AddUserInChatCommand request,
            CancellationToken cancellationToken)
        {
            var chat =
                await _dbContext.Chats.FirstOrDefaultAsync(Chat =>
                    Chat.Id == request.ChatId, cancellationToken);

            if (chat == null)
            {
                throw new NotFoundException(nameof(Domain.Chat), request.ChatId);
            }

            var user = await _dbContext.Users.FirstAsync(x => x.Id == request.UserId);
            chat.Users.Add(user);

            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
