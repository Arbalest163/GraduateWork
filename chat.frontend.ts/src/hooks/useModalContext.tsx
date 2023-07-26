import { useContext } from "react";
import ModalContext from "../modals/modal-provider";

const useModalContext = () => {
    return(
        useContext(ModalContext)
    );
}

export default useModalContext;