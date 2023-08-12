import { FC, ReactElement, useEffect, useState } from "react";
import { ChatLookupDto, FilterContext } from "../api/models/models";
import useSelectedChatContext from "../hooks/useSelectedChatContext";
import Connector from '../signalr-connection';

interface ChatListProps {
    chats: ChatLookupDto[] | undefined;
    getListChats: () => void;
    filterContext: FilterContext;
}

const ChatListComponent : FC<ChatListProps> = ({chats, getListChats, filterContext}) : ReactElement => {
    const {selectedChatId, setSelectedChatId} = useSelectedChatContext();
    const {joinChatGroup, leaveChatGroup} = Connector();

    useEffect(() => {
        getListChats();
    }, [filterContext]);
    
    useEffect(() => {
    const intervalId = setInterval(() => {
        getListChats();
    }, 5000);

    return () => clearInterval(intervalId);
    }, []);

    const handleSelectedChat = (chatId: string) => {
        if(selectedChatId && selectedChatId !== chatId) {
            leaveChatGroup(selectedChatId);
        }
        if(selectedChatId !== chatId) {
            setSelectedChatId(chatId);
            joinChatGroup(chatId);
        }
    }

    return (
        <div>
            {chats?.map((chat) => (
              <div key={chat.id} className="pointer-hover chat-list-item"
                onClick={() => handleSelectedChat(chat.id)}>
                    <div className="logo-container">
                        <img className="logo-50-c" src={chat.chatLogo} />
                    </div>
                    <div className={ `${chat.isCreatorChat 
                                ? "creator-chat" 
                                : ""}
                                ${chat.id === selectedChatId ? 'selected' : ''}
                                chat-list-title`}>
                                    {chat.title}
                    </div>
              </div>
            ))}
        </div>
    );
}

export default ChatListComponent;