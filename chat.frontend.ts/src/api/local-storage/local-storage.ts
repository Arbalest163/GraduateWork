import { CurrentUser } from "../models/models";

export const saveToken = (token: string | null | undefined) => {
    localStorage.setItem('token', token ? token : '');
}

export const saveRefreshToken = (refreshToken: string | null | undefined) => {
    localStorage.setItem('refresh_token', refreshToken ? refreshToken : '');
}
export const deleteToken = () => {
    localStorage.removeItem('token');
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