using Chat.Application.Common.Exceptions;

namespace Chat.Application.Chats.Commands.CreateMessage;

public class CreateMessageCommandHandler
 : IRequestHandler<CreateMessageCommand, string>
{
    private readonly IChatDbContext _dbContext;

    public CreateMessageCommandHandler(IChatDbContext dbContext) =>
        _dbContext = dbContext;

    public async Task<string> Handle(CreateMessageCommand request,
        CancellationToken cancellationToken)
    {
        var chat = await _dbContext.Chats.Include(x => x.Users).FirstOrDefaultAsync(x => x.Id == request.ChatId);
        
        if (chat is null)
        {
            throw new NotFoundException(nameof(Domain.Chat), request.ChatId);
        }

        //Для отладки сделал так
        var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == request.UserId) ?? throw new UnauthorizedAccessException();


        //if(chat.Users.Any(u => u.Id == request.UserId.ToString()) is false)
        //{
        //    throw new Exception("Вам не разрешено писать в данный чат.");
        //}

        //var user = chat.Users.First(x => x.Id == request.UserId.ToString());
        var message = new Message
        {
            User = user,
            Text = request.Message,
            DateSendMessage = DateTimeOffset.UtcNow,
        };

        chat.Messages.Add(message);

        if(!chat.Users.Any(x => x.Id == user.Id))
        {
            chat.Users.Add(user);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);

        return message.Text;
    }
}
