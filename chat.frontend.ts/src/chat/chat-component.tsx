import React, { FC, ReactElement, useEffect, useState } from 'react';
import { FormControl } from 'react-bootstrap';
import chatClient from '../api/clients/chat-client';
import './chat-component.css';
import { ChatLookupDto, SearchField, ApiError, FilterContext, ChatListVm } from '../api/models/models';
import { useNavigate } from 'react-router-dom';
import ActionChatsComponent from '../components/action-chats-component';
import MenuBurger from '../images/menu-burger';
import useModalContext from '../hooks/useModalContext';
import ChatCreateComponent from './chat-create-component';
import { ErrorModal } from '../modals/error-modal-component';
import ChatItemInfoComponent from '../components/chat-item-info-component';
import EditProfileComponent from '../components/edit-profile-component';
import ChangePasswordComponent from '../components/change-password-component';
import { getFilterContext, saveFilterContext, saveSelectedChat } from '../api/local-storage/local-storage';
import ChatListComponent from '../components/chat-list-component';

const ChatComponent: FC<{}> = (): ReactElement => {
  const {openModal} = useModalContext();
  const [chats, setChats] = useState<ChatLookupDto[] | undefined>(undefined);
  const [selectedChatId, setSelectedChatId] = useState<string | undefined>(undefined);
  const [filterContext, setChatFilter] = useState<FilterContext>(getFilterContext());

  const navigate = useNavigate();

  const handleChangeFilter = (filterText: string) => {
    const filter = {
      searchInfo: { 
        searchField: SearchField.Chats, 
        searchText: filterText, 
    }};
    saveFilterContext(filter);
    setChatFilter(filter);
  };

  const openErrorModal = (errorMessage: string) => {
    openModal(ErrorModal(errorMessage));
  }

  const getListChats = () => {
    const filterContext = getFilterContext();
    chatClient.getListChats(filterContext)
      .then((chatListVm) => {
        setChats(chatListVm.chats);
      })
      .catch((error : ApiError) => {
        if(error.message) {
          openErrorModal(error.message);
        }
        navigate('/auth/login');
      });
  }

  const handleCreateChat = () => {
    openModal(<ChatCreateComponent updateChats={getListChats}/>);
  }

  const handleOpeningProfile = () => {
    openModal(<EditProfileComponent/>);
  }

  const handleChangePassword = () => {
    openModal(<ChangePasswordComponent/>);
  }

  const actionChatsButtons = [
      {label: 'Создать чат', onClick: handleCreateChat},
      {label: 'Открыть профиль', onClick: handleOpeningProfile},
      {label: 'Сменить пароль', onClick: handleChangePassword},
    ];

  return (
    <div className='chat-container'>
      <div className="chats-container">
        <div className='chats-top-container'>
          <ActionChatsComponent buttons={actionChatsButtons} imageMenuButton={() => <MenuBurger/>}/>
          <div className="filter-input-container">
            <FormControl 
            placeholder='Поиск' 
            className='input' 
            onChange={(e) => handleChangeFilter(e.target.value)}/>
          </div>
        </div>
        <div className='chats-bottom-container'>
          <ChatListComponent chatId={selectedChatId} selectChat={setSelectedChatId} chats={chats} getListChats={getListChats} filterContext={filterContext}/>
        </div>
      </div>
      <div className="message-container">
        {selectedChatId && <ChatItemInfoComponent chatId={selectedChatId} updateChats={getListChats}/>}
      </div>
    </div>
  );
};

export default ChatComponent;
