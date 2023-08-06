import { ReactElement, FC, useState } from "react";
import { ApiError, CreateMessageDto } from "../api/models/models";
import chatClient from "../api/clients/chat-client";
import useModalContext from "../hooks/useModalContext";
import { ErrorModal } from "../modals/error-modal-component";
import ChatHeaderInfoComponent from "./chat-header-info-component";
import ChatMessageGroupsComponent from "./chat-message-groups-component";
import ChatMessageInputComponent from "./chat-message-input";

interface ChatItemInfoProps {
    chatId: string;
    updateChats: () => void;
}

const ChatItemInfoComponent: FC<ChatItemInfoProps> = ({chatId, updateChats}) : ReactElement => {
    const [messageInput, setMessageInput] = useState<string>('');
    const {openModal} = useModalContext();

    const openErrorModal = (errorMessage: string) => {
        openModal(ErrorModal(errorMessage));
    }

    const handleMessageSend = () => {
        if(messageInput) {
            let query: CreateMessageDto = {
                chatId: chatId,
                message: messageInput
            };
            chatClient.createMessage(query)
                .then(() => {
                    setMessageInput('');
                })
                .catch((error: ApiError) => {
                    if(error.message)
                    openErrorModal(error.message);
                });
        }
    }

    return (
        <div className="chat-item-container">
            <ChatHeaderInfoComponent chatId={chatId} updateChats={updateChats}/>
            <ChatMessageGroupsComponent chatId={chatId}/>
            <ChatMessageInputComponent 
                messageInput={messageInput} 
                setMessageInput={setMessageInput} 
                handleMessageSend={handleMessageSend}
            />
        </div>
    )
}

export default ChatItemInfoComponent;