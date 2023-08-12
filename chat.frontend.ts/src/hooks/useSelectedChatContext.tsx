import { useContext } from "react";
import SelectedChatContext from "../chat/selected-chat-provider";

const useSelectedChatContext = () => {
    return(
        useContext(SelectedChatContext)
    );
}

export default useSelectedChatContext;