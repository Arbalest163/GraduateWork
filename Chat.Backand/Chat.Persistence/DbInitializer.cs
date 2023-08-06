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
        var date1 = DateTime.Now;
        var date2 = date1.AddDays(-1);
        var date3 = date1.AddDays(-2);
        var date4 = date1.AddDays(-3);
        var date5 = date1.AddDays(-4);

        foreach (var i in 0..10)
        {
            yield return new Message
            {
                Id = Guid.NewGuid(),
                DateSendMessage = date1,
                Text = $"Текстовое сообщение {i} {date1}",
                User = user,
                Chat = chat,
            };
            yield return new Message
            {
                Id = Guid.NewGuid(),
                DateSendMessage = date2,
                Text = $"Текстовое сообщение {i} {date2}",
                User = user,
                Chat = chat,
            };
            yield return new Message
            {
                Id = Guid.NewGuid(),
                DateSendMessage = date3,
                Text = $"Текстовое сообщение {i} {date3}",
                User = user,
                Chat = chat,
            };
            yield return new Message
            {
                Id = Guid.NewGuid(),
                DateSendMessage = date4,
                Text = $"Текстовое сообщение {i} {date4}",
                User = user,
                Chat = chat,
            };
            yield return new Message
            {
                Id = Guid.NewGuid(),
                DateSendMessage = date5,
                Text = $"Текстовое сообщение {i} {date5}",
                User = user,
                Chat = chat,
            };
        }
    }
}
