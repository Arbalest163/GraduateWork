import { ReactElement, createContext, useState } from "react";
import useValidationErrors from "../hooks/useValidationErrors";

interface ModalContext {
    visible: boolean;
    content: ReactElement | undefined;
    openModal: (content: ReactElement) => void;
    closeModal: () => void;
  };
  
  const ModalContext = createContext<ModalContext>({
    visible: false,
    content: undefined,
    openModal: () => {},
    closeModal: () => {},
  });
  
  export const ModalProvider = ({children} : {children: JSX.Element}) => {
    const [visible, setVisible] = useState<boolean>(false);
    const [content, setContent] = useState<ReactElement | undefined>(undefined);
    const { clearValidationError } = useValidationErrors();
    const openModal = (content: ReactElement) => {
        setContent(content);
        setVisible(true);
    }
    const closeModal = () => {
        clearValidationError();
        setVisible(false);
    }
  
    return(
      <ModalContext.Provider value={{visible, content, openModal, closeModal}}>
        {children}
      </ModalContext.Provider>
    );
  };
  
  export default ModalContext;