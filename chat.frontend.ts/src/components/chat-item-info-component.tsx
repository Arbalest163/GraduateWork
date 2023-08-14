import { ReactElement, FC, useState } from "react";
import { ApiError, CreateMessageDto } from "../api/models/models";
import chatClient from "../api/clients/chat-client";
import useModalContext from "../hooks/useModalContext";
import { ErrorModal } from "../modals/error-modal-component";
import ChatHeaderInfoComponent from "./chat-header-info-component";
import ChatMessageGroupsComponent from "./chat-message-groups-component";
import ChatMessageInputComponent from "./chat-message-input";
import useSelectedChatContext from "../hooks/useSelectedChatContext";

interface ChatItemInfoProps {
    updateChats: () => void;
}

const ChatItemInfoComponent: FC<ChatItemInfoProps> = ({updateChats}) : ReactElement => {
    const {selectedChatId} = useSelectedChatContext();
    
    return (
        selectedChatId 
        ? <div className="chat-item-container">
            <ChatHeaderInfoComponent chatId={selectedChatId} updateChats={updateChats}/>
            <ChatMessageGroupsComponent chatId={selectedChatId}/>
            <ChatMessageInputComponent />
        </div>
        : <div>Выберете кому написать...</div>
        
    )
}

export default ChatItemInfoComponent;