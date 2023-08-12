using Chat.Application.Common.Mappings;
using Chat.Application.Common;

namespace Chat.Application.Chats.Queries.GetEditChat;

public class EditChatVm : IMapWith<Domain.Chat>
{
    public string ChatLogo { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Domain.Chat, EditChatVm>()
            .ForMember(chatDto => chatDto.ChatLogo,
                opt => opt.MapFrom(chat => Converter.CreateBase64File(chat.ChatLogo)))
            ;
    }
}
