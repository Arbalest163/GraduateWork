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
  const [currentUser, setCurrentUser] = useState<CurrentUser | null>(getCurrentUserLocalStorage());
  
  useEffect(() => {
    saveCurrentUser(currentUser);
  }, []);

  return (
    <AuthContext.Provider value={{currentUser, setCurrentUser}}>
      {children}
    </AuthContext.Provider>
  );
}

export default AuthContext;

