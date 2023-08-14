import { Dialog } from "@headlessui/react";
import { FC, ReactElement, useEffect, useState } from "react";
import chatClient from "../api/clients/chat-client";
import { stringEquals } from "../api/common/common-components";
import { ApiError, EditChatVm } from "../api/models/models";
import useModalContext from "../hooks/useModalContext";
import useValidationErrors from "../hooks/useValidationErrors";
import { ErrorModal } from "../modals/error-modal-component";
import ChangeImageComponent from "./change-image-component";

interface ChatEditProps {
    chatId: string;
    updateChats: () => void;
}

const ChatEditComponent  : FC<ChatEditProps> = ({chatId, updateChats}) : ReactElement => {
    const {closeModal} = useModalContext();
    const [errorMessage, setErrorMessage] = useState<string | undefined>(undefined);
    const {openModal} = useModalContext();
    const [editChat, setEditChat] = useState<EditChatVm>({chatLogo: '', title: ''});

    useEffect(() => {
        chatClient.editChat(chatId)
            .then(editChat => {
                setEditChat(editChat);
            })
            .catch((error: ApiError) => {
                if (error.message) {
                    openErrorModal(error.message);
                  } else if(error.errorsValidation) {
                    setValidationErrors(error.errorsValidation);
                  }
            });
    }, []);

    const { validationErrors,
        setValidationErrors, 
        isAnyValidationError,
        clearValidationError } = useValidationErrors();

    const openErrorModal = (errorMessage: string) => {
        openModal(ErrorModal(errorMessage));
    }
        

    const renameChat = () => {
        const updateChat = {
            chatId: chatId,
            title: editChat.title,
            chatLogo: editChat.chatLogo
        }
        chatClient.updateChat(updateChat)
            .then(() => {
                updateChats();
                closeModal();
            })
            .catch((error: ApiError) => {
                if (error.message) {
                    openErrorModal(error.message);
                  } else if(error.errorsValidation) {
                    setValidationErrors(error.errorsValidation);
                  }
            });
    }

    const handleInputChange = (event : React.ChangeEvent<HTMLInputElement>) => {
        const { name, value } = event.target;
        setEditChat((prevChat) => ({
            ...prevChat,
            [name]: value,
        }));
        clearValidationError(name);
    };

    const handleChatLogoChange = (base64: string) => {
        setEditChat((prevChat) => ({
          ...prevChat,
          chatLogo: base64,
        }));
    };

    return (
        <div className="bg">
             <Dialog.Panel className="popup">
                <div className="modal-title">Редактировать чат</div>
                <ChangeImageComponent source={editChat.chatLogo} onImageChange={handleChatLogoChange}/>
                {
                    isAnyValidationError('title') 
                    && <div className='error-container'>
                        {validationErrors.map(err => stringEquals(err.propertyName, 'title') 
                        && <div key={err.errorMessage} className="error-message">{err.errorMessage}</div>)}
                    </div>
                } 
                <input
                    className={`input ${isAnyValidationError('firstname') ? 'error' : ''}`}
                    type="text"
                    id="title"
                    name="title"
                    placeholder="Название чата"
                    value={editChat.title}
                    maxLength={20}
                    onChange={handleInputChange}
                />
                <div className="button-panel">
                    <button className="button" onClick={renameChat}>Сохранить</button>
                    <button className="button" onClick={closeModal}>Отмена</button>
                </div>
                
            </Dialog.Panel>
        </div>
    );
}

export default ChatEditComponent;