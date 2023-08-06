import React, { FC, ReactElement, useRef, useEffect, useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import authClient from '../api/clients/auth-client';
import { Calendar } from "react-date-range";
import "react-date-range/dist/styles.css";
import "react-date-range/dist/theme/default.css";
import { ApiError, ErrorValidation, RegisterUserDto } from '../api/models/models';
import useModalContext from '../hooks/useModalContext';
import { stringEquals, stringNotEquals } from '../api/common/common-components';
import { ErrorModal } from '../modals/error-modal-component';
import useValidationErrors from '../hooks/useValidationErrors';

const RegistrationComponent : FC<{}> = (): ReactElement => {
  const {openModal} = useModalContext();
  const [user, setUser] = useState<RegisterUserDto>({
    username: '',
    nickname: '',
    password: '',
    confirmPassword: '',
  });

  const { validationErrors,
     setValidationErrors, 
     isAnyValidationError,
     clearValidationError } = useValidationErrors();


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

  const handleRegistration = () => {
    setDisabledButton(true);
    
    authClient
      .register(user)
        .then(() => {
          console.log("Regiter success");
          navigate('/auth/login');
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
    <div className='register-container'>
      <div className='register-form'>
      <div className='container-center'>
        <h2>Регистрация</h2>
      </div>
      <div className='container-input'>
          {
            isAnyValidationError('username') 
            && <div className='error-container'>
                {validationErrors.map(err => stringEquals(err.propertyName, 'username') 
                  && <div key={err.errorMessage} className="error-message">{err.errorMessage}</div>)}
               </div>
          } 
          <input
            className={`input ${isAnyValidationError('username') ? 'error' : ''}`}
            type="text"
            id="username"
            name="username"
            placeholder="Логин"
            value={user.username}
            maxLength={20}
            onChange={handleInputChange}
          />
        </div>
        <div className='container-input'>
        {
            isAnyValidationError('nickname') 
            && <div className='error-container'>
                {validationErrors.map(err => stringEquals(err.propertyName, 'nickname') 
                  && <div key={err.errorMessage} className="error-message">{err.errorMessage}</div>)}
               </div>
          } 
          <input
            className={`input ${isAnyValidationError('nickname') ? 'error' : ''}`}
            type="text"
            id="nickname"
            name="nickname"
            placeholder="Ник"
            value={user.nickname}
            maxLength={20}
            onChange={handleInputChange}
          />
        </div>
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
            value={user.password}
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
            value={user.confirmPassword}
            maxLength={20}
            onChange={handleInputChange}
          />
        </div>
        <div className='container-center'>
          <button className='button' onClick={handleRegistration} disabled={isDisabledButton}>Зарегистрироваться</button>
        </div>
        <div className='container-center'>
          <p>
            Уже есть аккаунт? <Link to="/auth/login">Войти</Link>
          </p>
      </div>
      </div>
    </div>
  );
};

export default RegistrationComponent;
