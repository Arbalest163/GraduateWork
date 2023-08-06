import axios, { AxiosHeaders, AxiosInstance, AxiosRequestConfig, AxiosResponse, InternalAxiosRequestConfig } from 'axios';
import mem from "mem";
import { deleteTokens, getRefreshToken, getToken, saveRefreshToken, saveToken } from '../local-storage/local-storage';
import { Token } from '../models/models';

const headers = {
  "Content-Type": "application/json",
  Accept: 'application/json',
};

export class $AxiosHttpRequest {
  private baseUrl: string;
  private axiosInstance: AxiosInstance;

  constructor(baseUrl: string) {
    this.axiosInstance = axios.create({
      baseURL: baseUrl,
      headers: headers,
    });
    this.axiosInstance.interceptors.request.use(
      this.requestInterceptor, error => Promise.reject(error));

    this.axiosInstance.interceptors.response.use(
      response => response, this.responseInterceptor
    );
    this.baseUrl = baseUrl;
  }

  requestInterceptor = (config : InternalAxiosRequestConfig) => {
    const token = getToken();
    if(token) {
      config.headers.Authorization = `Bearer ${token}`;
    } else {
      delete config.headers.Authorization;
    }

    return config;
  }

  responseInterceptor = async (error : any) => {
    const config = error?.config;

    if (error?.response?.status === 401) {
      const token =  await this.memoizedRefreshToken();

      if(token) {
        if (token?.accessToken) {
          config.headers.Authorization = `Bearer ${token?.accessToken}`;
        } else {
          delete config?.headers.Authorization;
        }
        return axios(config);
      }
    }

    return Promise.reject(error);
  }

  get(url: string, queryParams?: Record<string, any>): Promise<AxiosResponse> {
    const fullUrl = this.buildUrl(url, queryParams);
    return this.axiosInstance.get(fullUrl);
  }

  post(url: string, body?: any, queryParams?: Record<string, any>): Promise<AxiosResponse> {
    const content = JSON.stringify(body ??= {});
    const fullUrl = this.buildUrl(url, queryParams);
    return this.axiosInstance.post(fullUrl, content);
  }

  postForm(url: string, formData: FormData) : Promise<AxiosResponse> {
    const config : AxiosRequestConfig<FormData> =  { headers: {
      "Content-Type": "multipart/form-data",
      Accept: 'application/json',
    }};
    return this.axiosInstance.postForm(url, formData, config);
  }

  putForm(url: string, formData: FormData) : Promise<AxiosResponse> {
    const config : AxiosRequestConfig<FormData> =  { headers: {
      "Content-Type": "multipart/form-data",
      Accept: 'application/json',
    }};
    return this.axiosInstance.putForm(url, formData, config);
  }

  delete(url: string, queryParams?: Record<string, any>): Promise<AxiosResponse> {
    const fullUrl = this.buildUrl(url, queryParams);
    return this.axiosInstance.delete(fullUrl);
  }

  put(url: string, body: any, queryParams?: Record<string, any>): Promise<AxiosResponse> {
    const content = JSON.stringify(body ??= {});
    const fullUrl = this.buildUrl(url, queryParams);

    return this.axiosInstance.put(fullUrl, content);
  }

  private buildQueryParams(params: Record<string, any>, parentKey?: string): string {
    const queryParts: string[] = [];

    for (const key in params) {
      if (params.hasOwnProperty(key)) {
        const value = params[key];
        const paramKey = parentKey ? `${parentKey}.${key}` : key;

        if (typeof value === 'object' && value !== null) {
          queryParts.push(this.buildQueryParams(value, paramKey));
        } else {
          const encodedKey = encodeURIComponent(paramKey);
          const encodedValue = encodeURIComponent(value);
          queryParts.push(`${encodedKey}=${encodedValue}`);
        }
      }
    }

    return queryParts.join('&');
  }

  private buildUrl(url: string, queryParams?: Record<string, any>): string {
    const searchParams = queryParams ? this.buildQueryParams(queryParams) : '';
    return `${this.baseUrl}/${url}${searchParams ? '?' + searchParams : ''}`;
  }

  private refreshTokenFn = async () : Promise<Token | null> => {
    const http = axios.create({
      baseURL: this.baseUrl,
      headers: headers,
    });
    const refreshToken = getRefreshToken();
    const response = await http.post("auth/refresh-token", {refreshToken: refreshToken});
    const token : Token = response.data;
    if(token) {
      saveToken(token.accessToken);
      saveRefreshToken(token.refreshToken);
    } else {
      deleteTokens();
    }
    
    return token;
  }

  private memoizedRefreshToken = mem(this.refreshTokenFn, {maxAge: 10000});
}
