using Chat.Application.Common.Mappings;
namespace Chat.Application.Chats.Queries.GetChatList;

public class ChatLookupDto : IMapWith<Domain.Chat>
{
    public Guid Id { get; set; }
    public string Title { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Domain.Chat, ChatLookupDto>()
            .ForMember(chatDto => chatDto.Id,
                opt => opt.MapFrom(chat => chat.Id))
            .ForMember(chatDto => chatDto.Title,
                opt => opt.MapFrom(chat => chat.Title))
        ;
    }
}
