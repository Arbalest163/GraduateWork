import { CurrentUser, FilterContext, SearchField } from "../models/models";

export const saveToken = (token: string | null | undefined) => {
    localStorage.setItem('token', token ? token : '');
}

export const getToken = () => {
    return localStorage.getItem('token');
}

export const saveRefreshToken = (refreshToken: string | null | undefined) => {
    localStorage.setItem('refresh_token', refreshToken ? refreshToken : '');
}

export const getRefreshToken = () : string | null => {
    return localStorage.getItem('refresh_token');
}

export const deleteToken = () => {
    localStorage.removeItem('token');
}

export const deleteTokens = () => {
    localStorage.removeItem('token');
    localStorage.removeItem('refresh_token');
}

export const saveCurrentUser = (currentUser: CurrentUser | null) => {
    let user = JSON.stringify(currentUser);
    localStorage.setItem('current_user', user ? user : '');
}

export const clearCurrentUser = () => {
    localStorage.setItem('current_user', '');
}

export const getCurrentUserLocalStorage = () : CurrentUser | null => {
    const user = localStorage.getItem('current_user');
    if(user) {
        const currentUser : CurrentUser =  JSON.parse(user);
        return currentUser;
    }
    return null;
}

export const saveSelectedChat = (chatId: string | undefined) => {
    localStorage.setItem('selectedChatId', chatId ? chatId : '');
}

export const getSelectedChat = () => {
    return localStorage.getItem('selectedChatId') ?? undefined
}

export const getFilterContext = () : FilterContext => {
    const filterContext = localStorage.getItem('filterContext');
    
    if(filterContext){
        return JSON.parse(filterContext);
    } else {
        const filterContextDefault = {searchInfo: { 
            searchField: SearchField.Chats, 
            searchText: '', 
        }};
        return filterContextDefault;
    }
}

export const saveFilterContext = (filterContext: FilterContext) => {
    const filter = JSON.stringify(filterContext);
    localStorage.setItem('filterContext', filter);
}