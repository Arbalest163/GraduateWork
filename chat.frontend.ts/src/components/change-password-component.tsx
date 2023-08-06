import { Dialog } from "@headlessui/react";
import { FC, ReactElement, useEffect, useState } from "react"
import useValidationErrors from "../hooks/useValidationErrors";
import { stringEquals } from "../api/common/common-components";
import { ApiError, ChangePasswordDto, EditUserDto } from "../api/models/models";
import authClient from "../api/clients/auth-client";
import { ErrorModal } from "../modals/error-modal-component";
import useModalContext from "../hooks/useModalContext";

const ChangePasswordComponent : FC<{}> = () : ReactElement => {
    const {openModal, closeModal} = useModalContext();
    const [passwordDto, setPasswordDto] = useState<ChangePasswordDto>({
        password: '',
        confirmPassword: '',
      });

    useEffect(() => {
        return clearValidationError();
    }, [])

    const { validationErrors,
        setValidationErrors, 
        isAnyValidationError,
        clearValidationError } = useValidationErrors();

    const [isDisabledButton, setDisabledButton] = useState<boolean>(false);

    const handleInputChange = (event : React.ChangeEvent<HTMLInputElement>) => {
        const { name, value } = event.target;
        setPasswordDto((prevPass) => ({
            ...prevPass,
            [name]: value,
        }));
        clearValidationError(name);
    };


    const handleSaveProfile = () => {
        setDisabledButton(true);
        authClient
            .changePassword(passwordDto)
            .then(() => {
                closeModal();
            })
            .catch((error : ApiError) => {
                if (error.message) {
                    openModal(ErrorModal(error.message));
                } else if(error.errorsValidation) {
                    setValidationErrors(error.errorsValidation);
                }
                
                setDisabledButton(false);
            });
    };
    return (
        <div className="bg">
        <Dialog.Panel className="popup">
        <div className='container-input'>
        {
            isAnyValidationError('password') 
            && <div className='error-container'>
                {validationErrors.map(err => stringEquals(err.propertyName, 'password') 
                  && <div key={err.errorMessage} className="error-message">{err.errorMessage}</div>)}
               </div>
          } 
          <input
            className={`input ${isAnyValidationError('password') ? 'error' : ''}`}
            type="password"
            id="password"
            name="password"
            placeholder="Пароль"
            value={passwordDto.password}
            maxLength={20}
            onChange={handleInputChange}
          />
        </div>
        <div className='container-input'>
        {
            isAnyValidationError('confirmPassword') 
            && <div className='error-container'>
                {validationErrors.map(err => stringEquals(err.propertyName, 'confirmPassword') 
                  && <div key={err.errorMessage} className="error-message">{err.errorMessage}</div>)}
               </div>
          } 
          <input
            className={`input ${isAnyValidationError('confirmPassword') ? 'error' : ''}`}
            type="password"
            id="confirmPassword"
            name="confirmPassword"
            placeholder="Подтверждение пароля"
            value={passwordDto.confirmPassword}
            maxLength={20}
            onChange={handleInputChange}
          />
        </div>
           <div className="button-panel">
               <button className="button" onClick={handleSaveProfile} disabled={isDisabledButton}>Сохранить</button>
               <button className="button" onClick={closeModal}>Отмена</button>
           </div>
       </Dialog.Panel>
   </div>)
}

export default ChangePasswordComponent;