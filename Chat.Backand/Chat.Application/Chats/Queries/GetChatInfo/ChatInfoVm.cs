using Chat.Application.Common.Mappings;

namespace Chat.Application.Chats.Queries.GetChatInfo;

public class ChatInfoVm : IMapWith<Domain.Chat>
{
    public string Title { get; set; }
    public Guid UserId { get; set; }
    public bool HasRightToEdit { get; set; }


    public void Mapping(Profile profile)
    {
        profile.CreateMap<Domain.Chat, ChatInfoVm>()
            .ForMember(chatDto => chatDto.UserId,
                opt => opt.MapFrom(chat => chat.User.Id))
            ;
    }
}
