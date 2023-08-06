using Chat.Application.Common.Mappings;

namespace Chat.Application.Chats.Queries.GetChat;

public class ChatMessageDto : IMapWith<Message>
{
    public Guid Id { get; set; }
    public string Text { get; set; }
    public ChatUserDto User { get; set; }
    public DateTime DateSendMessage { get; set; }
    public string TimeSendMessage { get; set; }
    public bool IsCreatorMessage { get; set; }
    public bool HasRightToEdit { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Message, ChatMessageDto>()
            .ForMember(messageDto => messageDto.Id,
                opt => opt.MapFrom(message => message.Id))
            .ForMember(messageDto => messageDto.Text,
                opt => opt.MapFrom(message => message.Text))
            .ForMember(messageDto => messageDto.User,
                opt => opt.MapFrom(message => message.User))
            .ForMember(messageDto => messageDto.DateSendMessage,
                opt => opt.MapFrom(message => message.DateSendMessage.LocalDateTime.Date))
            .ForMember(messageDto => messageDto.TimeSendMessage,
                opt => opt.MapFrom(message => message.DateSendMessage.LocalDateTime.ToString("HH:mm:ss")))
        ;
    }
}
