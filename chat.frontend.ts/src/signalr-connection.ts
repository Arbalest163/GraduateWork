import * as signalR from "@microsoft/signalr";
import { getToken } from "./api/local-storage/local-storage";
import { ReceiveMessage } from "./api/models/models";
const URL = process.env.HUB_ADDRESS ?? "http://192.168.2.114:5162/chat-hub";
class Connector {
    private connection: signalR.HubConnection;
    public onMessageReceivedEvent: (onMessageReceived: (message: ReceiveMessage) => void) => void;
    public onChatCountChangeEvent: (onChatCountChange: () => void) => void;
    static instance: Connector;
    constructor() {
        this.connection = new signalR.HubConnectionBuilder()
        .withUrl(URL, { accessTokenFactory: () => getToken() })
        .configureLogging(signalR.LogLevel.Information)
        .withAutomaticReconnect()
        .build();
        this.connection.start().catch(err => document.write(err));
        this.onMessageReceivedEvent = (onMessageReceived) => {
            this.connection.on("ReceiveMessage", (message: ReceiveMessage) => {
                onMessageReceived(message);
            });
        }
        this.onChatCountChangeEvent = (onChatCountChange) => {
            this.connection.on("ChatCountChange", () => {
                onChatCountChange();
            });
        }
    }

    public joinChatGroup = (chatId: string) => {
        if(this.connection.state === signalR.HubConnectionState.Connected)
            this.connection.send("JoinChatGroup", chatId).then(x => console.log(`Joined ${chatId} success`));
    }

    public leaveChatGroup = (chatId: string) => {
        if(this.connection.state === signalR.HubConnectionState.Connected)
            this.connection.send("LeaveChatGroup", chatId).then(x => console.log(`Leave ${chatId} success`));
    }
    
    public static getInstance(): Connector {
        return Connector.instance ??= new Connector();
    }
}
export default Connector.getInstance;