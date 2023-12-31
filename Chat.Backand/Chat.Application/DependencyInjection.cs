﻿namespace Chat.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
        services.AddValidatorsFromAssemblies(new[] { Assembly.GetExecutingAssembly() });
        services.AddTransient(typeof(IPipelineBehavior<,>),typeof(ValidationBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>),typeof(LoggingBehavior<,>));
        services.AddSignalR();
        return services;
    }
}
