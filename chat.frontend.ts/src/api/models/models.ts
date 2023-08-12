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
    chatLogo: string;
    isCreatorChat: boolean;
}

export interface MessageGroupsVm {
    messageGroups: MessageGroupDto[];
}

export interface MessageGroupDto {
    date: string;
    messages: ChatMessageDto[]
}

export interface ChatMessageDto {
    id: string;
    text: string;
    user: ChatUserDto;
    timeSendMessage: string;
    isCreatorMessage: boolean;
    hasRightToEdit: boolean;
}

export interface ReceiveMessage {
    id: string;
    text: string;
    user: ChatUserDto;
    date: string;
    timeSendMessage: string;
}

export interface UserBase {
    id: string;
    nickname: string;
    avatar: string;
    role: Role;
}

export interface CurrentUser extends UserBase {
    mainChats?: string[];
}

export interface ChatUserDto extends UserBase {
}

export interface ChatVm {
    id: string;
    title: string;
    groupMessages: MessageGroupDto[];
    users: ChatUserDto[];
    dateCreateChat: Date;
    isCreatorChat: boolean;
    hasRightToEdit: boolean;
}

export interface ChatInfoVm {
    title: string;
    hasRightToEdit: boolean;
}

export interface CreateChatDto {
    title: string;
    chatLogo: string;
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

export enum Role
{
    Admin = "Admin",
    Support = "Support",
    User = "User"
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
    nickname: string;
    password: string;
    confirmPassword: string;
}

export interface EditUserDto {
    avatar: string;
    firstname: string;
    lastname: string;
    middlename?: string;
    birthday: string;
}

export interface ChangePasswordDto {
    password: string;
    confirmPassword: string;
}

export enum SearchField {
    Users = "Users",
    Messages = "Messages",
    Chats = "Chats",
}

export interface Token {
    accessToken: string;
    refreshToken: string;
    expires: number;
}

export interface EditChatVm {
    chatLogo: string;
    title: string;
}

export interface UpdateChatDto {
    chatId: string;
    title?: string;
    chatLogo: string;
    deleteUser?: string;
    addUser?: string;
}

export interface FilterContext {
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