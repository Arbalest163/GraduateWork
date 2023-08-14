using Chat.Application.Common;
using Chat.Application.Common.Mappings;

namespace Chat.Application.Chats.Queries.GetChatDetails;

public class ChatDetailsVm : IMapWith<Domain.Chat>
{
    public string Title { get; set; }
    public string ChatLogo { get; set; }
    public ChatMemberDto[] ChatMembers { get; set; }
    public void Mapping(Profile profile)
    {
        profile.CreateMap<Domain.Chat, ChatDetailsVm>()
            .ForMember(chatDto => chatDto.Title,
                opt => opt.MapFrom(chat => chat.Title))
            .ForMember(chatDto => chatDto.ChatLogo,
                opt => opt.MapFrom(chat => Converter.CreateBase64File(chat.ChatLogo)))
            .ForMember(chatDto => chatDto.ChatMembers,
                opt => opt.MapFrom(chat => chat.Members))
            ;
    }
}
