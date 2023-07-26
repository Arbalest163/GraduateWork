import { ReactElement, FC, useRef, useState, useEffect } from "react";
import { ApiError, ChatVm, CreateMessageDto } from "../api/models/models";
import ActionChatsComponent from "./action-chats-component";
import MenuBurger from "../images/menu-burger";
import send from '../images/send.png'
import smile from '../images/smile.png'
import Picker from '@emoji-mart/react'
import data from '@emoji-mart/data'
import { FormControl } from "react-bootstrap";
import chatClient from "../api/clients/chat-client";
import useModalContext from "../hooks/useModalContext";
import { ErrorModal } from "../modals/error-modal-component";

interface ChatItemInfoProps {
    chatId: string;
}

const ChatItemInfoComponent: FC<ChatItemInfoProps> = ({chatId}) : ReactElement => {
    const {openModal} = useModalContext();
    const [showEmojiPicker, setShowEmojiPicker] = useState(false);
    const pickerTimeoutRef = useRef<NodeJS.Timeout | null>(null);
    const [messageInput, setMessageInput] = useState<string>('');
    const inputRef = useRef<HTMLInputElement>(null);
    const [chat, setChat] = useState<ChatVm | undefined>(undefined);

    const openErrorModal = (errorMessage: string) => {
        openModal(ErrorModal(errorMessage));
    }

    useEffect(() => {
        getChat();
    }, [chatId]);

    const getChat = () => {
    if (chatId){
        chatClient.getChat(chatId)
        .then((chat) => {
            setChat(chat);
        })
        .catch((error: ApiError) => {
            if(error.message)
            openErrorModal(error.message);
        });
    }
    }
    
    const handleMessageSend = () => {
        if(chat && messageInput){
        let query: CreateMessageDto = {
            chatId: chat.id,
            message: messageInput
        };
        chatClient.createMessage(query)
        .then(() => {
            setMessageInput('');
            getChat();
        })
        .catch((error: ApiError) => {
            if(error.message)
            openErrorModal(error.message);
        });
        }
    }

    const handleMessageSendEnter = (event: React.KeyboardEvent<HTMLInputElement>) => {
        if (event.key === 'Enter') {
        handleMessageSend();
        }
    };

    const handleMouseLeave = () => {
        if (pickerTimeoutRef.current) {
        clearTimeout(pickerTimeoutRef.current);
        }

        pickerTimeoutRef.current = setTimeout(() => {
        setShowEmojiPicker(false);
        }, 300);
    };

    const handleMouseEnterPicker = () => {
        if (pickerTimeoutRef.current) {
        clearTimeout(pickerTimeoutRef.current);
        }

        setShowEmojiPicker(true);
    };

    const handleEmojiPickup = (emoji: any) => {
        if (inputRef) {
        const cursorPosition = inputRef.current?.selectionStart || 0;
        setMessageInput((prevInput) => prevInput + emoji.native)

        const newCursorPosition = cursorPosition + emoji.native!.length;

        setTimeout(() => {
            inputRef.current?.setSelectionRange(
            newCursorPosition,
            newCursorPosition
            );
        }, 10);
        }
    };
    const actionChatsButtons = [
        {label: 'Пункт меню', onClick: () => {}},
        {label: 'Пункт меню', onClick: () => {}},
        {label: 'Пункт меню', onClick: () => {}},
      ];

    return (
        <div className="chat-item-container">
            <div className="chat-item-info">
                <div className="container-title">{chat?.title}</div>
                <ActionChatsComponent buttons={actionChatsButtons} imageMenuButton={() => <MenuBurger/>}/>
            </div>
            <div className="messages-container">
                {chat?.messages?.map((message) => (
                    <div className="message-item">
                        <span className="message-timestamp">{message.dateSendMessage}</span>
                        <span className='message-nick'> {message.user?.nickname}: </span>
                        {message.text}
                    </div>
                ))}
            </div>
            <div className="container-input">
                <FormControl
                    ref={inputRef}
                    placeholder='Написать сообщение...'
                    className="input"
                    value={messageInput}
                    onChange={(e) => setMessageInput(e.target.value)}
                    onKeyPress={handleMessageSendEnter}
                />
                <div className='button-input'>
                    <img className='image-input smile-button pointer-hover' src={smile} onMouseEnter={handleMouseEnterPicker} />
                    {showEmojiPicker && (
                        <div className='smile-picker' onMouseLeave={handleMouseLeave} onMouseEnter={handleMouseEnterPicker}>
                        <Picker
                            data={data}
                            onEmojiSelect={handleEmojiPickup}
                        />
                        </div>
                    )}
                    <img onClick={handleMessageSend} className='image-input pointer-hover' src={send} />
                </div>
            </div>
        </div>
    )
}

export default ChatItemInfoComponent;