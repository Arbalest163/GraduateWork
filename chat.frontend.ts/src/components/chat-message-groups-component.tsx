import { FC, ReactElement, useEffect, useRef, useState } from "react";
import chatClient from "../api/clients/chat-client";
import DownArrowButton from "../images/down-arrow";
import useModalContext from "../hooks/useModalContext";
import { ErrorModal } from "../modals/error-modal-component";
import { ApiError, MessageGroupDto } from "../api/models/models";
import DeleteButton from "../images/delete-button";

interface ChatMessageGroupsProps {
    chatId: string;
}

const ChatMessageGroupsComponent : FC<ChatMessageGroupsProps> = ({chatId}) : ReactElement => {
    const {openModal} = useModalContext();
    const [showScrollToDownButton, setShowScrollToDownButton] = useState(false);
    const messagesContainerRef = useRef<HTMLDivElement>(null);
    const [messageGroups, setMessageGroups] = useState<MessageGroupDto[] | undefined>(undefined);
    
    let isAtBottom = false;

    const openErrorModal = (errorMessage: string) => {
        openModal(ErrorModal(errorMessage));
    }

    const getChatMessageGroups = () => {
        chatClient.getMessageGroups(chatId)
            .then((messageGroups) => {
                setMessageGroups(messageGroups.messageGroups);
                if(isAtBottom) {
                    handleScrollToDownClick();
                }
            })
            .catch((error: ApiError) => {
                if(error?.message){
                    openErrorModal(error.message);
                }
            });
    }

    useEffect(() => {
        getChatMessageGroups();
        handleScrollToDownClick();
    }, [chatId]);

    const handleScroll = () => {
        if (messagesContainerRef.current) {
          const { scrollTop, clientHeight, scrollHeight } = messagesContainerRef.current;
          isAtBottom = scrollTop + clientHeight >= scrollHeight - 100;
          setShowScrollToDownButton(!isAtBottom);
        }
      };
    
    useEffect(() => {
        if (messagesContainerRef.current) {
            messagesContainerRef.current.addEventListener("scroll", handleScroll);
        }
        return () => {
            if (messagesContainerRef.current) {
                messagesContainerRef.current.removeEventListener("scroll", handleScroll);
            }
        };
    }, []);

    const handleScrollToDownClick = () => {
        if (messagesContainerRef.current) {
            messagesContainerRef.current.scrollTop = messagesContainerRef.current.scrollHeight;
        }
    };


    useEffect(() => {
        const intervalId = setInterval(() => {
            getChatMessageGroups();
        }, 300);

        return () => clearInterval(intervalId);
    }, [chatId]);

    

    const deleteMessage = (chatId: string) => {
        chatClient.deleteMessage(chatId).then(() => {
            getChatMessageGroups();
        })
    };

    return (
        <div className="message-groups-container" ref={messagesContainerRef}>
            {messageGroups?.map((group) => (
                <div key={group.date} className="group-messages-container">
                    <div className="group-date-container">
                        <div className="message-group-date">{group.date}</div>
                    </div>
                    <div className="container-for-messages">
                        {group.messages.map((message) => (
                            message.isCreatorMessage 
                            ? <div className={`container-for-message message-creator`} key={message.id}>
                                <div className="delete-message-container">
                                    {message.hasRightToEdit && 
                                    <div onClick={() => deleteMessage(message.id)}>
                                        <DeleteButton/>
                                    </div>}
                                </div>
                                <div className="bubble-message tail-right theme" data-time={message.timeSendMessage}>
                                    <div className="nickname">{message.user.nickname}</div>
                                    <div className="message-text">
                                        {message.text}
                                    </div>
                                </div>
                                
                                <div className="avatar-container">
                                    <img className="avatar-image" src={message.user.avatar} alt="" />
                                </div>
                                </div>
                            : <div className={`container-for-message message-other`} key={message.id}>
                                <div className="avatar-container">
                                    <img className="avatar-image" src={message.user.avatar} alt="" />
                                </div>
                                <div className="bubble-message tail-left theme" data-time={message.timeSendMessage}>
                                    <div className="nickname">{message.user.nickname}</div>
                                    <div className="message-text">
                                        {message.text}
                                    </div>
                                </div>
                                <div className="delete-message-container">
                                    {message.hasRightToEdit && 
                                    <div onClick={() => deleteMessage(message.id)}>
                                        <DeleteButton/>
                                    </div>}
                                </div>
                            </div>
                        ))}
                    </div>
                </div>
            ))}
            <svg height="10" width="10">
                <defs>
                    <clipPath id="tailLeft">
                        <path fill="none" d="M0,10 H10 V0 A10,10,0,0,1,0,10Z"/>
                    </clipPath>
                </defs>
            </svg>
            <svg height="10" width="10">
                <defs>
                    <clipPath id="tailRight">
                        <path fill="none" d="M0,0 V10 H10 A10,10,0,0,1,0,0Z"/>
                    </clipPath>
                </defs>
            </svg>
                <div className={`scroll-to-top-button ${showScrollToDownButton ? "active" : ""}`} onClick={handleScrollToDownClick}>
                    <DownArrowButton/>
                </div>
        </div>
    );
}
export default ChatMessageGroupsComponent;