using Chat.Application.Common.Mappings;

namespace Chat.Application.Chats.Queries.GetChatList;

public class MessageLookupDto : IMapWith<Message>
{
    public string Text { get; set; }
    public UserLookupDto User { get; set; }
    public DateTimeOffset DateSendMessage { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Message, MessageLookupDto>()
            .ForMember(messageDto => messageDto.Text,
                opt => opt.MapFrom(message => message.Text))
            .ForMember(messageDto => messageDto.User,
                opt => opt.MapFrom(message => message.User))
            .ForMember(messageDto => messageDto.DateSendMessage,
                opt => opt.MapFrom(message => message.DateSendMessage))
        ;
    }
}
