import send from '../images/send.png'
import smile from '../images/smile.png'
import Picker from '@emoji-mart/react'
import data from '@emoji-mart/data'
import { FC, ReactElement, useRef, useState } from "react";
import { FormControl } from "react-bootstrap";

interface ChatMessageInputProps {
    messageInput: string;
    setMessageInput: React.Dispatch<React.SetStateAction<string>>;
    handleMessageSend: () => void;
}

const ChatMessageInputComponent : FC<ChatMessageInputProps> 
= ({messageInput, setMessageInput, handleMessageSend}): ReactElement => {
    const inputRef = useRef<HTMLInputElement>(null);
    const pickerTimeoutRef = useRef<NodeJS.Timeout | null>(null);
    const [showEmojiPicker, setShowEmojiPicker] = useState(false);
    
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
        setMessageInput((prevValue) => prevValue + emoji.native)

        const newCursorPosition = cursorPosition + emoji.native!.length;

        setTimeout(() => {
            inputRef.current?.setSelectionRange(
            newCursorPosition,
            newCursorPosition
            );
        }, 100);
        }
    };
    
    return (
        <div className="container-input">
                <FormControl
                    ref={inputRef}
                    placeholder='Написать сообщение...'
                    className="input"
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