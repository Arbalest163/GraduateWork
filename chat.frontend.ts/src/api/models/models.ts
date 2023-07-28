export interface AuthQuery {
    login: string;
    password: string;
}

export interface ChatListVm {
    chats: ChatLookupDto[];
}

export interface ChatLookupDto {
    id: string;
    title: string;
}

export interface ChatMessageDto {
    text: string;
    user: ChatUserDto;
    dateSendMessage: string;
}

export interface UserBase {
    nickname: string;
}

export interface CurrentUser extends UserBase {
    mainChats?: string[];
}

export interface ChatUserDto extends UserBase {
}

export interface ChatVm {
    id: string;
    title: string;
    messages: ChatMessageDto[];
    users: ChatUserDto[];
    dateCreateChat: Date;
}

export interface CreateChatDto {
    title: string;
}

export interface CreateMessageDto {
    chatId: string;
    message: string;
}

export enum OrderField {
    Date = "Date",
    Title = "Title",
}

export enum OpeningDirection {
    DownRight = 'down-rigth',
    DownLeft = 'down-left',
    UpRight = 'up-right',
    UpLeft = 'up-left',
}

export interface ProblemDetails {
    type?: string | null;
    title?: string | null;
    status?: number | null;
    detail?: string | null;
    instance?: string | null;

    [key: string]: any;
}

export interface RegisterUserDto {
    username: string;
    firstname: string;
    lastname: string;
    middlename?: string;
    nickname: string;
    birthday: string;
    password: string;
    confirmPassword: string;
}

export enum SearchField {
    Users = "Users",
    Messages = "Messages",
    DateCreateChat = "DateCreateChat",
    Title = "Title",
}

export interface Token {
    accessToken: string;
    refreshToken: string;
    expires: number;
}

export interface UpdateChatDto {
    chatId: string;
    title?: string;
    deleteUser?: string;
    addUser?: string;
}

export interface ChatsFilter {
    userId?: string;
    orderInfo?: OrderInfo,
    searchInfo?: SearchInfo,
}

export interface OrderInfo {
    orderField?: OrderField,
    orderAscending?: boolean,
}

export interface SearchInfo {
    searchField?: SearchField,
    searchText?: string,
    dateCreateChat? : Date,
}

export interface ApiError {
    message?: string;
    errorsValidation?: ErrorValidation[];
}

export interface ErrorValidation {
    propertyName: string;
    errorMessage: string;
}

export class ApiException extends Error {
    override message: string;
    status: number;
    response: string;
    headers: { [key: string]: any; };
    result: any;

    constructor(message: string, status: number, response: string, headers: { [key: string]: any; }, result: any) {
        super();

        this.message = message;
        this.status = status;
        this.response = response;
        this.headers = headers;
        this.result = result;
    }

    protected isApiException = true;

    static isApiException(obj: any): obj is ApiException {
        return obj.isApiException === true;
    }
}

export interface Button {
    label: string;
    onClick: () => void;
}