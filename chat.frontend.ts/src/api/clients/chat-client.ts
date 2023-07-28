import { ChatsFilter, ChatListVm, ChatVm, CreateChatDto, UpdateChatDto, CreateMessageDto } from "../models/models";
import { ClientBase } from "./client-base";

const apiVersion = '1.0';

export class ChatClient extends ClientBase {
    constructor(api_version: string) {
      super(api_version);
    }

    getChats(chatsFilter: ChatsFilter): Promise<ChatListVm> {
        return this.$http.get("chats", chatsFilter)
            .then(this.handleResponse, this.handleError);
    }

    getChat(chatId: string): Promise<ChatVm> {
        return this.$http.get("chat", { chatId: chatId })
            .then(this.handleResponse, this.handleError);
    }

    createChat(body: CreateChatDto): Promise<string> {
        return this.$http.post("chat", body)
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
};

const chatClient = new ChatClient(apiVersion);
export default chatClient;