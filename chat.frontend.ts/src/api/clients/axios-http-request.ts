import axios, { AxiosInstance, AxiosRequestConfig, AxiosResponse } from 'axios';

export class $AxiosHttpRequest {
  private baseUrl: string;
  private http: AxiosInstance;

  constructor(baseUrl: string, http: AxiosInstance = axios) {
    this.http = http;
    this.baseUrl = baseUrl;
  }

  protected transformConfig(config: AxiosRequestConfig) {
    const token = localStorage.getItem('token');
    config.headers = {
        ...config.headers,
        Authorization: 'Bearer ' + token,
    };
    return Promise.resolve(config);
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

  async get(url: string, queryParams?: Record<string, any>): Promise<AxiosResponse> {
    const fullUrl = this.buildUrl(url, queryParams);

    let config: AxiosRequestConfig = {
      headers: {
        Accept: 'application/json'
      }
    };
    return this.transformConfig(config)
      .then(config => {
        return this.http.get(fullUrl, config);
      });
  }

  async post(url: string, body?: any, queryParams?: Record<string, any>): Promise<AxiosResponse> {
    const content = JSON.stringify(body ??= {});
    const fullUrl = this.buildUrl(url, queryParams);

    let config: AxiosRequestConfig = {
      method: "POST",
      headers: {
        'Content-Type': 'application/json-patch+json',
        Accept: 'application/json'
      }
    };

    return this.transformConfig(config)
      .then(config => {
        return this.http.post(fullUrl, content, config);
      });
  }

  async delete(url: string, queryParams?: Record<string, any>): Promise<AxiosResponse> {
    const fullUrl = this.buildUrl(url, queryParams);

    let config: AxiosRequestConfig = {
      headers: {}
    };

    return this.transformConfig(config)
      .then(config => {
        return this.http.delete(fullUrl, config);
      });
  }

  async put(url: string, body: any, queryParams?: Record<string, any>): Promise<AxiosResponse> {
    const content = JSON.stringify(body ??= {});
    const fullUrl = this.buildUrl(url, queryParams);

    let config: AxiosRequestConfig = {
      headers: {
        'Content-Type': 'application/json-patch+json',
        Accept: 'application/json'
      }
    };

    return this.transformConfig(config)
      .then(config => {
        return this.http.put(fullUrl, content, config);
      });
  }
}
