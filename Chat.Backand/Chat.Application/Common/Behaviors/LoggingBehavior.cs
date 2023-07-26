namespace Chat.Application.Common.Behaviors
{
    public class LoggingBehavior<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse> where TRequest
        : IRequest<TResponse>
    {
        private readonly IChatUserPrincipal _chatUserPrincipal;
        private readonly ILogger _logger;

        public LoggingBehavior(IChatUserPrincipal chatUserPrincipal, ILogger<LoggingBehavior<TRequest, TResponse>> logger) =>
            (_chatUserPrincipal, _logger) = (chatUserPrincipal, logger);

        public async Task<TResponse> Handle(TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;
            var userId = _chatUserPrincipal.UserId;

            _logger.LogInformation("Chat Request: {Name} UserId :{@UserId} {@Request}",
                requestName, userId, request);

            var response = await next();

            return response;
        }
    }
}
