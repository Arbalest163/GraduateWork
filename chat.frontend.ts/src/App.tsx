import { FC, ReactElement, useEffect, useState } from 'react';
import { Route, Routes, useNavigate } from 'react-router-dom';
import './App.css';
import ChatComponent from './chat/chat-component';
import RegistrationComponent from './auth/register-component';
import LoginComponent from './auth/login-component';
import StartUrlComponent from './start-url-component';
import LogoutComponent from './auth/logout-component';
import { Link } from 'react-router-dom';
import useAuthContext from './hooks/useAuthContext';
import ModalComponent from './modals/modal-component';
import { clearSelectedChat } from './api/local-storage/local-storage';


const App: FC<{}> = (): ReactElement => {
  const [isAuth, setIsAuth] = useState<boolean>(false);
  const {currentUser} = useAuthContext();

  useEffect(() => {
    clearSelectedChat();
    if(currentUser){
      setIsAuth(true);
    } else {
      setIsAuth(false);
    }
  }, [currentUser]);

  return (
      <div className='App'>
        <header>
          <div className='container-title'>Чат</div>
          <div className='logout-link'>
            {isAuth && <Link to="/auth/logout">Выход</Link>}
          </div>
        </header>
        <main>
            <Routes> 
              <Route path='/' Component={StartUrlComponent}/>
              <Route path='/auth/login' Component={LoginComponent}/>
              <Route path='/auth/register' Component={RegistrationComponent}/>
              <Route path='/chats' Component={ChatComponent}/>
              <Route path='/auth/logout' Component={LogoutComponent}/>
            </Routes>
            <ModalComponent/>
        </main>
        <footer>Дипломная работа для академии "Топ" г.Самара. Разработано на React TypeScript. Backand: С#. База данных: MSSQL. Разрабочик: Иван Гилязов. Email: evil_xacker@mail.ru</footer>
      </div>
  );
};

export default App;
