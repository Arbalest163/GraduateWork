import { FC, ReactElement, useEffect, useState } from "react";
import { ChatLookupDto, FilterContext } from "../api/models/models";

interface ChatListProps {
    chatId: string | undefined;
    selectChat: (chatId: string) => void;
    chats: ChatLookupDto[] | undefined;
    getListChats: () => void;
    filterContext: FilterContext;
}

const ChatListComponent : FC<ChatListProps> = ({chatId, selectChat, chats, getListChats, filterContext}) : ReactElement => {
    
    useEffect(() => {
        getListChats();
    }, [filterContext]);
    
    useEffect(() => {
    const intervalId = setInterval(() => {
        getListChats();
    }, 5000);

    return () => clearInterval(intervalId);
    }, []);

    return (
        <div>
            {chats?.map((chat) => (
              <div key={chat.id} 
                className={`${chat.isCreatorChat ? "creator-chat-item" : "chat-item"} pointer-hover ${chat.id === chatId ? 'selected' : ''}`} 
                onClick={() => selectChat(chat.id)}>
                {chat.title}
              </div>
            ))}
        </div>
    );
}

export default ChatListComponent;