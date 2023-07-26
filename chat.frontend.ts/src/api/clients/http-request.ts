export class $HttpRequest {
    private baseUrl: string;
    private http: { fetch(url: RequestInfo, init?: RequestInit): Promise<Response> };
  
    constructor(baseUrl: string, http?: { fetch(url: RequestInfo, init?: RequestInit): Promise<Response> }) {
      this.http = http ? http : (window as any);
      this.baseUrl = baseUrl;
    }

    private buildQueryParams(params: Record<string, any>, parentKey?: string): string {
        const queryParts: string[] = [];
      
        for (const key in params) {
          if (params.hasOwnProperty(key)) {
            const value = params[key];
            const paramKey = parentKey ? `${parentKey}.${key}` : key;
      
            if (typeof value === 'object'&& value !== null) {
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

    private buildUrl(url: RequestInfo, queryParams?: Record<string, any>): string {
        const searchParams = queryParams ? this.buildQueryParams(queryParams) : '';
        return `${this.baseUrl}/${url}${searchParams ? '?' + searchParams : ''}`;
    }
  
    get(url: RequestInfo, queryParams?: Record<string, any>) {
      const fullUrl = this.buildUrl(url, queryParams);
  
      let options: RequestInit = {
        method: "GET",
        headers: {
          "Accept": "application/json"
        }
      };
  
      return this.http.fetch(fullUrl, options);
    }
  
    post(url: RequestInfo, body?: any, queryParams?: Record<string, any>) {
      body ??= {};
      const content = JSON.stringify(body);
      const fullUrl = this.buildUrl(url, queryParams);
  
      let options: RequestInit = {
        body: content,
        method: "POST",
        headers: {
          "Content-Type": "application/json-patch+json",
          "Accept": "application/json",
        }
      };
  
      return this.http.fetch(fullUrl, options);
    }
  
    delete(url: RequestInfo, queryParams?: Record<string, any>) {
      const fullUrl = this.buildUrl(url, queryParams);
  
      let options: RequestInit = {
        method: "DELETE",
        headers: {}
      };
  
      return this.http.fetch(fullUrl, options);
    }
  
    put(url: RequestInfo, body: any, queryParams?: Record<string, any>) {
      body ??= {};
      const content = JSON.stringify(body);
      const fullUrl = this.buildUrl(url, queryParams);
  
      let options: RequestInit = {
        body: content,
        method: "PUT",
        headers: {
          "Content-Type": "application/json-patch+json",
          "Accept": "application/json",
        }
      };
  
      return this.http.fetch(fullUrl, options);
    }
  }
  