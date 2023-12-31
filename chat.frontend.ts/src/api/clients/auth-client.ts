import { getRefreshToken } from "../local-storage/local-storage";
import { AuthQuery, ChangePasswordDto, CurrentUser, EditUserDto, RegisterUserDto, Token } from "../models/models";
import { ClientBase } from "./client-base";

const apiVersion = '1.0';

export class AuthClient extends ClientBase {
    constructor(api_version: string) {
      super(api_version);
    }

    login(authQuery: AuthQuery) : Promise<Token> {
      return this.$http.post("auth/login", authQuery)
        .then(this.handleResponse, this.handleError);
    }

    register(body: RegisterUserDto) : Promise<void> {
      return this.$http.post("auth/register", body)
        .then(this.handleResponse, this.handleError);
    }

    getEdit() : Promise<EditUserDto> {
      return this.$http.get("auth/edit")
        .then(this.handleResponse, this.handleError);
    }

    edit(body: EditUserDto) : Promise<void> {
      return this.$http.put("auth/edit", body)
        .then(this.handleResponse, this.handleError);
    }

    changePassword(body: ChangePasswordDto) : Promise<void> {
      return this.$http.put("auth/change-password", body)
        .then(this.handleResponse, this.handleError);
    }

    logout() : Promise<string> {
      return this.$http.post("auth/logout")
        .then(this.handleResponse, this.handleError);
    }

    getUser() : Promise<CurrentUser> {
      return this.$http.get('auth/get-user')
        .then(this.handleResponse, this.handleError);
    }

    checkState() : Promise<boolean> {
      return this.$http.get('auth/check-state')
        .then(this.handleResponse, this.handleError);
    }

    refreshToken(refreshToken: string | null) : Promise<Token> {
      return this.$http.post('auth/refresh', { refreshToken: refreshToken })
        .then(this.handleResponse, this.handleError);
    }
};

const authClient = new AuthClient(apiVersion);
export default authClient;

export const refreshToken = () => {
  const refreshToken = getRefreshToken();
  return authClient.refreshToken(refreshToken);
}
    
