﻿using Chat.Application.Common.Mappings;

namespace Chat.Application.Chats.Queries.GetChat;

public class ChatVm : IMapWith<Domain.Chat>
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public IList<GroupMessagesDto> GroupMessages { get; set; }
    public IList<ChatMessageDto> Messages { get; set; }
    public IList<ChatUserDto> Users { get; set; }
    public string DateCreateChat { get; set; }
    public Guid UserId { get; set; }
    public bool IsCreatorChat { get; set; }
    public bool HasRightToEdit { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Domain.Chat, ChatVm>()
            .ForMember(chatDto => chatDto.Id,
                opt => opt.MapFrom(chat => chat.Id))
            .ForMember(chatDto => chatDto.Title,
                opt => opt.MapFrom(chat => chat.Title))
            .ForMember(chatDto => chatDto.Users,
                opt => opt.MapFrom(chat => chat.Members))
            .ForMember(chatDto => chatDto.UserId,
                opt => opt.MapFrom(chat => chat.User.Id))
            .ForMember(chatDto => chatDto.Messages,
                    opt => opt.MapFrom(chat => chat.Messages.OrderByDescending(x => x.DateSendMessage)))
            .ForMember(chatDto => chatDto.DateCreateChat,
                    opt => opt.MapFrom(chat => chat.DateCreateChat.LocalDateTime.ToString("dd.MM.yyyy HH:mm")))
        ;
    }
}
