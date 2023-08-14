import React, { FC, ReactElement, useEffect, useState } from 'react';
import { Link, useLocation, useNavigate } from 'react-router-dom';
import { ApiError, AuthQuery, ErrorValidation } from '../api/models/models';
import authClient from '../api/clients/auth-client';
import { deleteToken, saveRefreshToken, saveToken } from '../api/local-storage/local-storage';
import './login-component.css';
import ozero_gory_sosny from './ozero-gory-sosny.jpg'
import useAuthContext from '../hooks/useAuthContext';
import useModalContext from '../hooks/useModalContext';
import { stringEquals } from '../api/common/common-components';
import { ErrorModal } from '../modals/error-modal-component';
import useValidationErrors from '../hooks/useValidationErrors';
import Connector from '../signalr-connection';
import mem from 'mem';

const LoginComponent : FC<{}> = (): ReactElement => {
  const { setCurrentUser } = useAuthContext();
  const {openModal} = useModalContext();
  const navigate = useNavigate();
  const location = useLocation();
  const from = location.state?.from?.pathname || '/';
  const { validationErrors,
    setValidationErrors, 
    isAnyValidationError,
    clearValidationError } = useValidationErrors();

  const [authQuery, setAuthQuery] = useState<AuthQuery>({
    login: '',
    password: '',
  });

  const handleInputChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = event.target;
    setAuthQuery((prevAuthQuery) => ({
      ...prevAuthQuery,
      [name]: value,
    }));
    clearValidationError(name);
  };

  const openErrorModal = (errorMessage: string) => {
    openModal(ErrorModal(errorMessage));
  }

  const handleLogin = () => {

    authClient.login(authQuery)
      .then((token) => {
        saveToken(token.accessToken);
        saveRefreshToken(token.refreshToken);
        authClient.getUser().then((user) => {
          setCurrentUser(user);
          const _ = Connector();
          navigate('/chats', { replace: true });
        });
      })
      .catch((error : ApiError) => {
        setAuthQuery({login: '', password: ''});
        if (error.message) {
          openErrorModal(error.message);
        } else if(error.errorsValidation) {
          setValidationErrors(error.errorsValidation);
        }
      });
  };

  const memoizedHandleLogin = mem(handleLogin, {maxAge: 10000})

  useEffect(() => {
    deleteToken();
  });

  return (
    <div className='login-container'>
      <div className='auth-container'>
        <div className='auth-form'>
          <div className='container-center'>
            <h2 className='container-center'>Вход в чат</h2>
          </div>
            <div className='container-input'>
              {
                isAnyValidationError('login') 
                && <div className='error-container'>
                    {validationErrors.map(err => stringEquals(err.propertyName, 'login') 
                      && <div key={err.errorMessage} className="error-message">{err.errorMessage}</div>)}
                  </div>
              } 
              <input
                className={`input ${isAnyValidationError('login') ? 'error' : ''}`}
                type="text"
                id="login"
                name="login"
                value={authQuery.login}
                maxLength={20}
                placeholder="Введите логин"
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
                value={authQuery.password}
                maxLength={20}
                placeholder="Введите пароль"
                onChange={handleInputChange}
              />
            </div>
            <div className='container-center'>
              <button className='button' onClick={memoizedHandleLogin}>Войти</button>
            </div>
            <div className='container-center'>
              <p className='text-register'>
                Нет аккаунта? <Link to="/auth/register">Зарегистрироваться</Link>
              </p>
            </div>
        </div>
      </div>
      <div className='image-container'>
        <img className='image' src={ozero_gory_sosny}/>
      </div>
    </div>
  );
};

export default LoginComponent;

