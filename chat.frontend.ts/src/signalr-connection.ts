import * as signalR from "@microsoft/signalr";
import { getToken } from "./api/local-storage/local-storage";
import { ReceiveMessage } from "./api/models/models";
const URL = process.env.HUB_ADDRESS ?? "http://192.168.2.114:5162/chat-hub";
class Connector {
    private connection: signalR.HubConnection;
    public onMessageReceivedEvent: (onMessageReceived: (message: ReceiveMessage) => void) => void;
    public onInformationMessageRecievedEvent: (onInformationMessageReceived: (message: ReceiveMessage) => void) => void;
    public onChatCountChangeEvent: (onChatCountChange: () => void) => void;
    static instance: Connector;
    constructor() {
        this.connection = this.initConnection();

        this.connection.start()
        .then(() => console.log("Connect chat-hub success"))
        .catch(err => console.log(err));

        this.connection.onreconnecting((error) => {
            console.log('Reconnecting...', error);
            this.connection = this.initConnection();
        });

        this.onMessageReceivedEvent = (onMessageReceived) => {
            this.connection.on("ReceiveMessage", (message: ReceiveMessage) => {
                onMessageReceived(message);
            });
        }
        this.onInformationMessageRecievedEvent = (onInformationMessageReceived) => {
            this.connection.on("ReceiveInformationMessage", (message: ReceiveMessage) => {
                onInformationMessageReceived(message);
            });
        }
        this.onChatCountChangeEvent = (onChatCountChange) => {
            this.connection.on("OnChatCountChange", () => {
                onChatCountChange();
            });
        }
    }

    private initConnection = () => {
        const newToken = getToken();
        return new signalR.HubConnectionBuilder()
            .withUrl(URL, { accessTokenFactory: () => newToken })
            .configureLogging(signalR.LogLevel.Information)
            .withAutomaticReconnect([0, 1000, 2000, 3000, 4000, 5000])
            .build();
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