using Chat.Application.Common.Mappings;

namespace Chat.Application.Chats.Queries.GetChat;

public class ChatMessageDto : IMapWith<Message>
{
    public string Text { get; set; }
    public ChatUserDto User { get; set; }
    public string DateSendMessage { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Message, ChatMessageDto>()
            .ForMember(messageDto => messageDto.Text,
                opt => opt.MapFrom(message => message.Text))
            .ForMember(messageDto => messageDto.User,
                opt => opt.MapFrom(message => message.User))
            .ForMember(messageDto => messageDto.DateSendMessage,
                opt => opt.MapFrom(message => message.DateSendMessage.LocalDateTime.ToString("dd.MM.yyyy HH:mm:ss")))
        ;
    }
}
