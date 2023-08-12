import { FC, ReactElement } from "react";
import { ChatLookupDto } from "../api/models/models";
import useSelectedChatContext from "../hooks/useSelectedChatContext";
import Connector from '../signalr-connection';

interface ChatListProps {
    chats: ChatLookupDto[];
}

const ChatListComponent : FC<ChatListProps> = ({chats}) : ReactElement => {
    const {selectedChatId, setSelectedChatId} = useSelectedChatContext();
    const {joinChatGroup, leaveChatGroup} = Connector();

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