import { AxiosError, AxiosResponse } from "axios";
import { $AxiosHttpRequest } from "./axios-http-request";
import { ApiError, ErrorValidation, Token } from "../models/models";
import { getRefreshToken, saveCurrentUser, saveToken } from "../local-storage/local-storage";
import { refreshToken } from "./auth-client";

const baseUrlKeenetic = 'https://chatapi.noragami.keenetic.link';
const baseUrlLocal = 'http://192.168.2.114:5162';

export class ClientBase {
  protected $http: $AxiosHttpRequest;
  protected baseUrl: string;

  constructor(api_version: string, baseUrl?: string) {
    this.baseUrl = baseUrl ? baseUrl : `${baseUrlLocal}/api/${api_version}`;
    this.$http = new $AxiosHttpRequest(this.baseUrl);
  }

  protected handleResponse(response: AxiosResponse): Promise<any> {
    return Promise.resolve(response.data);
  }

  protected handleError(error: AxiosError): Promise<never> {
    const response = error.response;

    if (response) {
      if(response.status === 401) {
        saveCurrentUser(null);
      }
      let data = JSON.stringify(response.data);
      let apiError: ApiError = JSON.parse(data);
      return Promise.reject(apiError);
    } else {
      return Promise.reject(error);
    }
  }
}

