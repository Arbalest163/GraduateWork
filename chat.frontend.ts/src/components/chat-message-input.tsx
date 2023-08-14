import send from '../images/send.png'
import smile from '../images/smile.png'
import Picker from '@emoji-mart/react'
import data from '@emoji-mart/data'
import { FC, ReactElement, useRef, useState } from "react";
import { ApiError, CreateMessageDto } from '../api/models/models';
import useSelectedChatContext from '../hooks/useSelectedChatContext';
import chatClient from '../api/clients/chat-client';


const ChatMessageInputComponent : FC<{}> 
= (): ReactElement => {
    const inputRef = useRef<HTMLTextAreaElement>(null);
    const {selectedChatId} = useSelectedChatContext();
    const pickerTimeoutRef = useRef<NodeJS.Timeout | null>(null);
    const [showEmojiPicker, setShowEmojiPicker] = useState(false);

    const [messageInput, setMessageInput] = useState<string>('');


    const handleMessageSend = () => {
        if(messageInput && selectedChatId) {
            let query: CreateMessageDto = {
                chatId: selectedChatId,
                message: messageInput
            };
            chatClient.createMessage(query)
                .then(() => {
                    setMessageInput('');
                })
                .catch((error: ApiError) => {
                    if(error.message)
                    console.log(error.message);
                });
        }
    }
    
    const handleMessageSendEnter = (event: React.KeyboardEvent<HTMLTextAreaElement>) => {
        if (event.key === 'Enter' && !event.shiftKey) {
            event.preventDefault();
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
            setMessageInput((prevValue) => prevValue + emoji.native)
        }
    };
    
    return (
        <div className="container-input">
            <textarea
                ref={inputRef}
                rows={1}
                placeholder='Написать сообщение...'
                className="input-textarea"
                value={messageInput}
                onChange={(e) => setMessageInput(e.target.value)}
                onKeyDown={handleMessageSendEnter}
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
    );
}

export default ChatMessageInputComponent;