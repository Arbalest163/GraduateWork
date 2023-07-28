import { createContext, useEffect, useState } from 'react';
import { CurrentUser } from '../api/models/models';
import { getCurrentUserLocalStorage, saveCurrentUser } from '../api/local-storage/local-storage';

type AuthContextType = {
  currentUser: CurrentUser | null,
  setCurrentUser: (user: CurrentUser | null) => void;
};

const AuthContext = createContext<AuthContextType>({
  currentUser: null,
  setCurrentUser: () => {},
});

export const AuthContextProvider = ({children} : {children: JSX.Element}) => {
  const [currentUser, setUser] = useState<CurrentUser | null>(getCurrentUserLocalStorage());

  const setCurrentUser = (user :CurrentUser | null) => {
    setUser(user);
    saveCurrentUser(user);
  }

  return (
    <AuthContext.Provider value={{currentUser, setCurrentUser}}>
      {children}
    </AuthContext.Provider>
  );
}

export default AuthContext;

