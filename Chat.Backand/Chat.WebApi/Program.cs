using Chat.Application;
using Chat.Application.Common.Mappings;
using Chat.Application.Interfaces;
using Chat.Persistence;
using Chat.WebApi;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Chat.WebApi.Middleware;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using Newtonsoft.Json.Converters;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Newtonsoft.Json.Serialization;
using Chat.Application.Hubs;
using Microsoft.AspNetCore.SignalR;

var builder = WebApplication.CreateBuilder(args);

RegisterServices(builder.Services);
ConfigureAuthentication(builder.Services);

var app = builder.Build();

await Configure(app);

app.Run();

const string CorsPolicy = "chat-policy";

void RegisterServices(IServiceCollection services)
{
    services.AddAutoMapper(config =>
    {
        config.AddProfile(new AssemblyMappingProfile(Assembly.GetExecutingAssembly()));
        config.AddProfile(new AssemblyMappingProfile(typeof(IChatDbContext).Assembly));
    });

    services.AddApplication();
    services.AddPersistence(builder.Configuration);
    
    services.ConfigureApplicationCookie(config =>
    {
        config.Cookie.Name = "Chat.Api.Cookie";
        config.LoginPath = "/auth/login";
        config.LogoutPath = "/auth/logout";
    });

    services.AddHttpContextAccessor();

    services.AddControllers()
       .AddNewtonsoftJson(o =>
       {
           o.SerializerSettings.Converters.Add(new StringEnumConverter());
           o.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
           o.SerializerSettings.Converters.Add(new IsoDateTimeConverter());
       });

    builder.Services.AddCors(options =>
    {
        options.AddPolicy(name: CorsPolicy, policy =>
        {
            policy
                //.WithOrigins("https://chatapp.noragami.keenetic.link", "http://chatapp.noragami.keenetic.link", "http://192.168.2.114", "https://192.168.2.114:5163", "http://localhost:3000")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .SetIsOriginAllowed(origin => true)
                .AllowCredentials()
                ;
        });
    });

    services.AddVersionedApiExplorer(options =>
                options.GroupNameFormat = "'v'VVV");
    services.AddTransient<IConfigureOptions<SwaggerGenOptions>,
            ConfigureSwaggerOptions>();
    services.AddSwaggerGen();
    services.AddSwaggerGenNewtonsoftSupport();
    services.AddApiVersioning();
}

async Task Configure(WebApplication app)
{
    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }

    using var scope = app.Services.CreateScope();

    var provider = scope.ServiceProvider.GetRequiredService<IApiVersionDescriptionProvider>();

    app.UseSwagger();
    app.UseSwaggerUI(config =>
    {
        foreach (var description in provider.ApiVersionDescriptions)
        {
            config.SwaggerEndpoint(
                $"/swagger/{description.GroupName}/swagger.json",
                description.GroupName.ToUpperInvariant());
            config.RoutePrefix = "swagger";
        }
    });
    
    var context = scope.ServiceProvider.GetRequiredService<ChatDbContext>();
    var passwordManager = scope.ServiceProvider.GetRequiredService<IPasswordManager>();
    await DbInitializer.Initialize(context, passwordManager);

    app.UseRouting();
    app.UseStaticFiles();
    app.UseCustomExceptionHandler();
    app.UseRequestTimingMiddleware();
    //app.UserCusomCorsMiddleware();
    app.UseCors(CorsPolicy);
    app.UseAuthentication();
    app.UseAuthorization();
    
    app.MapControllers();
    app.MapHub<ChatHub>("/chat-hub");
    //var chatsContext = scope.ServiceProvider.GetRequiredService<IHubContext<ChatHub, IChatClient>>();
    //var chatIds = context.Chats.Select(x => x.Id.ToString()).ToArray();
    //foreach (var chatId in chatIds)
    //{
    //    await chatsContext.Groups.AddToGroupAsync("", chatId);
    //}
}

void ConfigureAuthentication(IServiceCollection services)
{
    services.AddAuthorization();
    services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = Configuration.Instance.TokenValidationParameters;
        options.RequireHttpsMetadata = false;

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];

                var path = context.HttpContext.Request.Path;
                if (!string.IsNullOrEmpty(accessToken) &&
                    (path.StartsWithSegments("/chat-hub")))
                {
                    context.Token = accessToken;
                }
                return Task.CompletedTask;
            }
        };
    });
}
