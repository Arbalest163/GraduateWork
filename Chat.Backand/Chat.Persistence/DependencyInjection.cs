using Chat.Application.Interfaces;
using Chat.Persistence.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System.Diagnostics;

namespace Chat.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection
        services, IConfiguration configuration)
    {
        var connectionString = configuration["DbConnection"];
        services.AddDbContext<ChatDbContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });
        services.AddScoped<IChatDbContext>(provider =>
            provider.GetRequiredService<ChatDbContext>());

        services.AddSingleton((sp) => GetConnection(configuration));
        services.AddSingleton((sp) => sp.GetRequiredService<IConnectionMultiplexer>().GetDatabase());

        services.AddScoped(provider => LoadChatApiOptions(configuration));

        services.AddSingleton<IChatUserPrincipal, ChatUserPrincipal>();
        services.AddTransient<IPasswordManager, PasswordManager>();
        services.AddScoped<IJwtTokensService, JwtTokensService>();
        services.AddScoped<IFileManager, FileManager>();

        return services;
    }

    private static IConnectionMultiplexer GetConnection(IConfiguration configuration)
    {
        var config = GetRedisConfig(configuration);
        return ConnectionMultiplexer.Connect(config);
    }

    private static ConfigurationOptions GetRedisConfig(IConfiguration configuration)
    {
        var connString = configuration["RedisConnection"];
        if (string.IsNullOrEmpty(connString))
        {
            throw new Exception("RedisConnection is empty");
        }
        var config = ConfigurationOptions.Parse(connString);
        var processId = Process.GetCurrentProcess().Id;
        config.AbortOnConnectFail = false;
        config.ClientName = Environment.MachineName + " - " + AppDomain.CurrentDomain.FriendlyName + " (" + processId + ")";
        return config;
    }

    private static ChatApiOptions LoadChatApiOptions(IConfiguration configuration)
    {
        var chatApiOptions = configuration.GetSection("ChatApiSettings");
        if (chatApiOptions == null)
        {
            throw new Exception("Configuration section 'ChatApiSettings' not defined");
        }

        var options = new ChatApiOptions();
        chatApiOptions.Bind(options);

        return options;
    }
}
