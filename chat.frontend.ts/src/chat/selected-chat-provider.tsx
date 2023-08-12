import { createContext, useEffect, useState } from 'react';
import { getSelectedChat, saveSelectedChat } from '../api/local-storage/local-storage';

type SelectedChatType = {
  selectedChatId: string | undefined,
  setSelectedChatId: (chatId: string) => void;
};

const SelectedChatContext = createContext<SelectedChatType>({
    selectedChatId: undefined,
    setSelectedChatId: () => {},
});

export const SelectedChatContextProvider = ({children} : {children: JSX.Element}) => {
  const [selectedChatId, setSelectedChat] = useState<string | undefined>(getSelectedChat());

  const setSelectedChatId = (chatId: string) => {
    saveSelectedChat(chatId);
    setSelectedChat(chatId);
  }

  return (
    <SelectedChatContext.Provider value={{selectedChatId, setSelectedChatId}}>
      {children}
    </SelectedChatContext.Provider>
  );
}

export default SelectedChatContext;