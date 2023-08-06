using Chat.Application.Common.Mappings;

namespace Chat.Application.Chats.Queries.GetMessageGroups;

public class MessageLookupDto : IMapWith<Message>
{
    public Guid Id { get; set; }
    public string Text { get; set; }
    public UserLookupDto User { get; set; }
    public DateTime DateSend { get; set; }
    public DateTime TimeSend { get; set; }
    public string TimeSendMessage { get; set; }
    public bool IsCreatorMessage { get; set; }
    public bool HasRightToEdit { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Message, MessageLookupDto>()
            .ForMember(messageDto => messageDto.Id,
                opt => opt.MapFrom(message => message.Id))
            .ForMember(messageDto => messageDto.Text,
                opt => opt.MapFrom(message => message.Text))
            .ForMember(messageDto => messageDto.User,
                opt => opt.MapFrom(message => message.User))
            .ForMember(messageDto => messageDto.DateSend,
                opt => opt.MapFrom(message => message.DateSendMessage.LocalDateTime.Date))
            .ForMember(messageDto => messageDto.TimeSend,
                opt => opt.MapFrom(message => message.DateSendMessage.LocalDateTime.ToLocalTime()))
            .ForMember(messageDto => messageDto.TimeSendMessage,
                opt => opt.MapFrom(message => message.DateSendMessage.LocalDateTime.ToString("HH:mm:ss")))
        ;
    }
}
