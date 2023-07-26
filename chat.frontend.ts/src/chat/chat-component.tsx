import React, { FC, ReactElement, useEffect, useState } from 'react';
import { FormControl } from 'react-bootstrap';
import chatClient from '../api/clients/chat-client';
import './chat-component.css';
import { ChatLookupDto, ChatsFilter, SearchField, ApiError } from '../api/models/models';
import { useNavigate } from 'react-router-dom';
import ActionChatsComponent from '../components/action-chats-component';
import MenuBurger from '../images/menu-burger';
import useModalContext from '../hooks/useModalContext';
import ChatCreateComponent from './chat-create-component';
import { ErrorModal } from '../modals/error-modal-component';
import ChatItemInfoComponent from '../components/chat-item-info-component';

const ChatComponent: FC<{}> = (): ReactElement => {
  const {openModal} = useModalContext();
  const [chats, setChats] = useState<ChatLookupDto[] | undefined>(undefined);
  const [selectedChat, setSelectedChat] = useState<ChatLookupDto | undefined>(undefined);
  const [chatsFilter, setChatFilter] = useState<ChatsFilter | null>(null);
 
  const navigate = useNavigate();

  useEffect(() => {
    getChats();
  }, [chatsFilter]);
  
  useEffect(() => {
    getChats();
  }, []);

  useEffect(() => {
    const intervalId = setInterval(() => {
      getChats();
    }, 5000);

    return () => clearInterval(intervalId);
  }, []);

  const handleChangeFilter = (filterText: string) => {
    if (filterText?.length !== 0) {
      const chatsFilter = {
        searchInfo: { 
          searchField: SearchField.Title, 
          searchText: filterText 
        }
      };
      setChatFilter(chatsFilter);
    } else {
      setChatFilter(null);
    }
  };

  const openErrorModal = (errorMessage: string) => {
    openModal(ErrorModal(errorMessage));
  }

  const getChats = () => {
    chatClient.getChats(chatsFilter ?? undefined)
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
    openModal(<ChatCreateComponent/>);
  }

  const actionChatsButtons = [
      {label: 'Создать чат', onClick: () => handleCreateChat()},
      {label: 'Пункт меню', onClick: () => {}},
      {label: 'Пункт меню', onClick: () => {}},
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
          <div>
            {chats?.map((chat) => (
              <div key={chat.id} 
                className={`chat-item pointer-hover ${chat.id === selectedChat?.id ? 'selected' : ''}`} 
                onClick={() => setSelectedChat(chat)}>
                {chat.title}
              </div>
            ))}
          </div>
        </div>
      </div>
      <div className="message-container">
        {selectedChat && <ChatItemInfoComponent chatId={selectedChat.id}/>}
      </div>
    </div>
  );
};

export default ChatComponent;
