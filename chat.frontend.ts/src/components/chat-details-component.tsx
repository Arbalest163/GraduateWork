import { FC, ReactElement, useEffect, useState } from "react";
import chatClient from "../api/clients/chat-client";
import { ChatDetailsVm } from "../api/models/models";
import { Dialog } from "@headlessui/react";

interface ChatDetailsProps {
    chatId: string;
}

const ChatDetailsComponent: FC<ChatDetailsProps> = ({chatId}) : ReactElement => {
    const [chatDetails, setChatDetails] = useState<ChatDetailsVm | null>(null);
    const getChatDetails = () => {
        chatClient.getChatDetails(chatId).then(chatDetailsVm => {
            setChatDetails(chatDetailsVm);
        })
    }

    useEffect(() => {
        getChatDetails();
    },[])

    return (
        <div className="bg">
             <Dialog.Panel className="chat-details-popup">
                <div className="chat-details-container">
                    <div className="chat-details-header">
                        <img className="chat-details-logo" src={chatDetails?.chatLogo} alt="" />
                        <div className="chat-simple-info-container">
                            <div className="chat-details-title">{chatDetails?.title}</div>
                            <div className="chat-details-count-members">{chatDetails?.chatMembers.length} участников</div>
                        </div>
                    </div>
                    <div className="chat-details-members-container">
                        <div className="chat-details-members-title">Участники: </div>
                        {chatDetails?.chatMembers?.map(member => (
                            <div className="chat-details-member">
                            <img className="chat-details-member-avatar" src={member.avatar} alt="" />
                            <div className="chat-details-member-nickname">
                                {member.nickname}
                            </div>
                        </div>
                        ))}
                    </div>
                </div>
            </Dialog.Panel>
        </div>
        );
}

export default ChatDetailsComponent;