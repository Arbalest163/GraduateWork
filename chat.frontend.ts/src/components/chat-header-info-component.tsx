import { FC, ReactElement, useEffect, useState } from "react";
import ActionChatsComponent from "./action-chats-component";
import MenuBurger from "../images/menu-burger";
import { Button, ChatInfoVm } from "../api/models/models";
import chatClient from "../api/clients/chat-client";
import ChatEditComponent from "./chat-edit-component";
import useModalContext from "../hooks/useModalContext";

interface ChatHeaderInfoProps {
    chatId: string;
    updateChats: () => void;
}

const ChatHeaderInfoComponent: FC<ChatHeaderInfoProps> = ({chatId, updateChats}) : ReactElement => {
    const [chatInfo, setChatInfo] = useState<ChatInfoVm | null>(null);
    const {openModal} = useModalContext();
    const deleteChat = () => {
        chatClient.deleteChat(chatId)
            .then(() => {
                setChatInfo(null);
                updateChats();
            });
    }

    const editChat = () => {
        openModal(<ChatEditComponent chatId={chatId} updateChats={updateChats}/>);
    }

    const getDefaultButtons = () => {
        return [
            {label: 'Информация о чате', onClick: () => {}},
          ];
    }

    const [buttons, setButtons] = useState<Button[]>(getDefaultButtons());

    const deleteButton = {
        label: 'Удалить чат',
        onClick: deleteChat,
    };

    const renameChatButton = {
        label: 'Редактировать чат',
        onClick: editChat
    };

    const getChatInfo = () => {
        chatClient.getChatInfo(chatId).then((chatInfo) => {
            setChatInfo(chatInfo);
        })
    }

    useEffect(() => {
        const buttons = getDefaultButtons();
        if(chatInfo?.hasRightToEdit) {
            buttons.push(deleteButton);
            buttons.push(renameChatButton);
        }

        setButtons(buttons);
    }, [chatInfo])

    useEffect(() => {
        getChatInfo();
    }, [chatId]);

    


    return (
        <div className="chat-item-info">
                <div className="container-title">{chatInfo?.title}</div>
                <ActionChatsComponent buttons={buttons} imageMenuButton={() => <MenuBurger/>}/>
        </div>
    );
}

export default ChatHeaderInfoComponent;