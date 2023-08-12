import { FC, ReactElement, useState } from "react";
import { Dialog } from "@headlessui/react";
import useModalContext from "../hooks/useModalContext";
import chatClient from "../api/clients/chat-client";
import { ApiError, CreateChatDto } from "../api/models/models";
import useValidationErrors from "../hooks/useValidationErrors";
import { stringEquals } from "../api/common/common-components";
import { ErrorModal } from "../modals/error-modal-component";
import ChangeImageComponent from "../components/change-image-component";

const ChatCreateComponent : FC<{}> = () : ReactElement => {
    const {closeModal} = useModalContext();
    const [errorMessage, setErrorMessage] = useState<string | undefined>(undefined);
    const {openModal} = useModalContext();
    const [createChatDto, setCreateChatDto] = useState<CreateChatDto>({chatLogo: '', title: ''});

    const { validationErrors,
        setValidationErrors, 
        isAnyValidationError,
        clearValidationError } = useValidationErrors();

    const openErrorModal = (errorMessage: string) => {
        openModal(ErrorModal(errorMessage));
    }
        

    const createChat = () => {
        chatClient.createChat(createChatDto)
            .then(() => {
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
        setCreateChatDto((prevChat) => ({
            ...prevChat,
            [name]: value,
        }));
        clearValidationError(name);
    };

    const handleChatLogoChange = (base64: string) => {
        setCreateChatDto((prevChat) => ({
          ...prevChat,
          chatLogo: base64,
        }));
    };

    return (
        <div className="bg">
             <Dialog.Panel className="popup">
                <div className="modal-title">Создать чат</div>
                <ChangeImageComponent source={createChatDto.chatLogo} onImageChange={handleChatLogoChange}/>
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
                    value={createChatDto.title}
                    maxLength={20}
                    onChange={handleInputChange}
                />
                <div className="button-panel">
                    <button className="button" onClick={createChat}>Создать</button>
                    <button className="button" onClick={closeModal}>Отмена</button>
                </div>
                
            </Dialog.Panel>
        </div>
    );
}

export default ChatCreateComponent;
