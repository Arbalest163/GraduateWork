using Chat.Application.Interfaces;
using Chat.Domain;
using Chat.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Chat.Persistence;

public class DbInitializer
{
    public static async Task Initialize(ChatDbContext context, IPasswordManager passwordManager)
    {
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
        context.Database.Migrate();

        await context.UserRoles.AddRangeAsync(GetRoles());
        await context.SaveChangesAsync();

        var userRole = context.UserRoles.First(x => x.Role == Role.User);
        var adminRole = context.UserRoles.First(x => x.Role == Role.Admin);
        var users = GetUsers(userRole).ToList();
        users.ForEach(async x => x.PasswordHash = await passwordManager.GetHashPassword(x.UserName));
        var admin = GetAdmin(adminRole);
        admin.PasswordHash = await passwordManager.GetHashPassword(admin.UserName);
        await context.Users.AddRangeAsync(users);
        await context.Users.AddAsync(admin);
        await context.SaveChangesAsync();
    }

    private static IEnumerable<UserRole> GetRoles()
    {
        yield return new UserRole { Id = Guid.NewGuid(), Role = Role.Admin, RoleName = "Администратор" };
        yield return new UserRole { Id = Guid.NewGuid(), Role = Role.Support, RoleName = "Поддержка" };
        yield return new UserRole { Id = Guid.NewGuid(), Role = Role.User, RoleName = "Пользователь" };
    }

    private static User GetAdmin(UserRole role)
    {
        return new User
        {
            Id = Guid.NewGuid(),
            UserName = $"Admin",
            Firstname = $"Admin",
            Lastname = $"Admin",
            Middlename = $"Admin",
            Nickname = $"Admin",
            UserRole = role,
            Birthday = DateTime.Now,

        };
    }
    private static IEnumerable<User> GetUsers(UserRole role)
    {
        foreach(var i in 0..20)
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                UserName = $"TestUsername{i}",
                Firstname = $"TestFirstname{i}",
                Lastname = $"TestLastname{i}",
                Middlename = $"TestModdlename{i}",
                Nickname = $"TestNickname{i}",
                UserRole = role,
                Birthday = DateTime.Now,

            };
            var chat = new Domain.Chat
            {
                Id = Guid.NewGuid(),
                DateCreateChat = DateTime.Now,
                Title = $"Тестовый чат {i}",
                User = user,
                IsActive = true,
            };
            var messages = GetMessages(user, chat);
            chat.Messages.AddRange(messages);
            user.MemberChats = new[] { chat };
            yield return user;
        }
    }

    private static IEnumerable<Message> GetMessages(User user, Domain.Chat chat)
    {
        foreach(var i in 0..5)
        {
            yield return new Message
            {
                Id = Guid.NewGuid(),
                DateSendMessage = DateTime.Now,
                Text = $"Текстовое сообщение {i}",
                User = user,
                Chat = chat,
            };
        }
    }
}
