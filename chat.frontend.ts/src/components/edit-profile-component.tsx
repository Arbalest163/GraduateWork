import { Dialog } from "@headlessui/react";
import { FC, ReactElement, useEffect, useState } from "react"
import useValidationErrors from "../hooks/useValidationErrors";
import { stringEquals } from "../api/common/common-components";
import { useNavigate } from "react-router-dom";
import { ApiError, EditUserDto } from "../api/models/models";
import authClient from "../api/clients/auth-client";
import { ErrorModal } from "../modals/error-modal-component";
import { Calendar } from "react-date-range";
import useModalContext from "../hooks/useModalContext";
import ChangeImageComponent from "./change-image-component";

const EditProfileComponent : FC<{}> = () : ReactElement => {
    const {openModal, closeModal} = useModalContext();
    const [user, setUser] = useState<EditUserDto>({
        avatar: '',
        firstname: '',
        lastname: '',
        middlename: '',
        birthday: '',
      });

    useEffect(() => {
        authClient.getEdit()
            .then(editUser => {
                setUser(editUser);
            })
    }, []);
    
    const { validationErrors,
        setValidationErrors, 
        isAnyValidationError,
        clearValidationError } = useValidationErrors();

    const today = new Date();
    const minDate = new Date(
    today.getFullYear() - 14, 
    today.getMonth(), 
    today.getDate()
    );

    const [birthday, setBirthday] = useState<Date>(minDate);
    const [isCalendarActive, setIsCalendarActive] = useState(false);

    const navigate = useNavigate();

    const [isDisabledButton, setDisabledButton] = useState<boolean>(false);

    const handleInputChange = (event : React.ChangeEvent<HTMLInputElement>) => {
        const { name, value } = event.target;
        setUser((prevUser) => ({
            ...prevUser,
            [name]: value,
        }));
        clearValidationError(name);
    };

    const setDate = (date: Date) => {
        setBirthday(date);
        user.birthday = date.toLocaleDateString();
        closeCalendar();
    }

    const openCalendar = () => {
        setIsCalendarActive(true);
    }

    const closeCalendar = () => {
        setTimeout(() => {
            setIsCalendarActive(false);
        }, 100);
    }

    const onClickToDate = () => {
        clearValidationError('birthday');
        setIsCalendarActive(!isCalendarActive);
    }

    const handleSaveProfile = () => {
        setDisabledButton(true);

        authClient
            .edit(user)
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

    const handleAvatarChange = (base64: string) => {
      setUser((prevUser) => ({
        ...prevUser,
        avatar: base64,
      }));
    };

    if (!user) {
      return <div>Loading...</div>;
    }
    return (
        <div className="bg">
          <Dialog.Panel className="popup">
            <ChangeImageComponent source={user.avatar} onImageChange={handleAvatarChange} description="Avatar"/>
            <div className='container-input'>
              {
                isAnyValidationError('firstname') 
                && <div className='error-container'>
                    {validationErrors.map(err => stringEquals(err.propertyName, 'firstname') 
                      && <div key={err.errorMessage} className="error-message">{err.errorMessage}</div>)}
                  </div>
              } 
              <input
                className={`input ${isAnyValidationError('firstname') ? 'error' : ''}`}
                type="text"
                id="firstname"
                name="firstname"
                placeholder="Имя"
                value={user.firstname}
                maxLength={20}
                onChange={handleInputChange}
              />
            </div>
            <div className='container-input'>
              {
                isAnyValidationError('lastname') 
                && <div className='error-container'>
                    {validationErrors.map(err => stringEquals(err.propertyName, 'lastname') 
                      && <div key={err.errorMessage} className="error-message">{err.errorMessage}</div>)}
                  </div>
              } 
              <input
                className={`input ${isAnyValidationError('lastname') ? 'error' : ''}`}
                type="text"
                id="lastname"
                name="lastname"
                placeholder="Фамилия"
                value={user.lastname}
                maxLength={20}
                onChange={handleInputChange}
              />
            </div>
            <div className='container-input'>
            {
                isAnyValidationError('middlename') 
                && <div className='error-container'>
                    {validationErrors.map(err => stringEquals(err.propertyName, 'middlename') 
                      && <div key={err.errorMessage} className="error-message">{err.errorMessage}</div>)}
                  </div>
              } 
              <input
                className={`input ${isAnyValidationError('middlename') ? 'error' : ''}`}
                type="text"
                id="middlename"
                name="middlename"
                placeholder="Отчество"
                value={user.middlename}
                maxLength={20}
                onChange={handleInputChange}
              />
            </div>
            <div className="container-input">
            {
                isAnyValidationError('birthday') 
                && <div className='error-container'>
                    {validationErrors.map(err => stringEquals(err.propertyName, 'birthday') 
                      && <div key={err.errorMessage} className="error-message">{err.errorMessage}</div>)}
                  </div>
              } 
              <input
                className={`input ${isAnyValidationError('birthday') ? 'error' : ''}`}
                type="text"
                id="birthday"
                name="birthday"
                placeholder="Дата рождения"
                value={user.birthday || ''}
                onClick={onClickToDate}
              />
            </div>
            <div className='container-date'>
            {isCalendarActive &&
                  <Calendar date={birthday} onChange={setDate} />
              }
            </div>
            <div className="button-panel">
                  <button className="button" onClick={handleSaveProfile} disabled={isDisabledButton}>Сохранить</button>
                  <button className="button" onClick={closeModal}>Отмена</button>
            </div>
        </Dialog.Panel>
   </div>)
}

export default EditProfileComponent;