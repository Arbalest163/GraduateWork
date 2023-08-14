import { FilterContext, ChatListVm, ChatVm, CreateChatDto, UpdateChatDto, CreateMessageDto, ChatInfoVm, MessageGroupsVm, EditChatVm, ChatDetailsVm } from "../models/models";
import { ClientBase } from "./client-base";

const apiVersion = '1.0';

export class ChatClient extends ClientBase {
    constructor(api_version: string) {
      super(api_version);
    }

    getListChats(filterContext: FilterContext): Promise<ChatListVm> {
        return this.$http.get("chat/list", filterContext)
            .then(this.handleResponse, this.handleError);
    }

    getChat(chatId: string): Promise<ChatVm> {
        return this.$http.get("chat", { chatId: chatId })
            .then(this.handleResponse, this.handleError);
    }

    getChatInfo(chatId: string): Promise<ChatInfoVm> {
        return this.$http.get("chat/info", { chatId: chatId })
            .then(this.handleResponse, this.handleError);
    }

    checkChatAccess(chatId: string): Promise<boolean> {
        return this.$http.get("chat/access", { chatId: chatId })
            .then(this.handleResponse, this.handleError);
    }

    createChat(body: CreateChatDto): Promise<string> {
        return this.$http.post("chat", body)
            .then(this.handleResponse, this.handleError);
    }

    editChat(chatId: string) : Promise<EditChatVm> {
        return this.$http.get("chat/edit", { chatId: chatId })
            .then(this.handleResponse, this.handleError);
    }

    updateChat(body?: UpdateChatDto): Promise<void> {
        return this.$http.put("chat", body)
            .then(this.handleResponse, this.handleError);
    }

    deleteChat(chatId: string): Promise<void> {
        return this.$http.delete("chat", { chatId: chatId })
            .then(this.handleResponse, this.handleError);
    }

    createMessage(body: CreateMessageDto): Promise<string> {
        return this.$http.post("chat/message", body)
            .then(this.handleResponse, this.handleError);
    }

    getMessageGroups(chatId: string) : Promise<MessageGroupsVm> {
        return this.$http.get("chat/message-groups", { chatId: chatId })
            .then(this.handleResponse, this.handleError);
    }

    deleteMessage(messageId: string) : Promise<void> {
        return this.$http.delete("chat/message", { messageId: messageId })
            .then(this.handleResponse, this.handleError);
    }

    uploadFile(formData: FormData) : Promise<string> {
        return this.$http.postForm("chat/upload-file", formData)
            .then(this.handleResponse, this.handleError);
    }

    joinChat(chatId: string) : Promise<void> {
        return this.$http.post("chat/join-chat", { chatId: chatId })
            .then(this.handleResponse, this.handleError);
    }

    getChatDetails(chatId: string): Promise<ChatDetailsVm> {
        return this.$http.get("chat/details", {chatId: chatId})
            .then(this.handleResponse, this.handleError);
    }
};

const chatClient = new ChatClient(apiVersion);
export default chatClient;