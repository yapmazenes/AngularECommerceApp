import { Inject, Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http'
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class HttpClientService {

  constructor(private httpClient: HttpClient, @Inject("baseUrl") private baseUrl: string) { }

  private url(requestParameters: Partial<RequestParameters>): string {
    return `${requestParameters.baseUrl ? requestParameters.baseUrl : this.baseUrl}/${requestParameters.controller}${requestParameters.action ? `/${requestParameters.action}` : ''}`;
  }

  private getQueryString(queryString?: string): string {
    return queryString ? `?${queryString}` : '';
  }

  private getIdParameter(id?: string): string {
    return id ? `/${id}` : '';
  }

  get<T>(requestParameters: Partial<RequestParameters>, id?: string): Observable<T> {
    let url: string = "";
    if (requestParameters.customEndPoint)
      url = requestParameters.customEndPoint;
    else
      url = `${this.url(requestParameters)}${this.getIdParameter(id)}${this.getQueryString(requestParameters.queryString)}`;

    return this.httpClient.get<T>(url, { headers: requestParameters.headers });
  }

  post<T>(requestParameters: Partial<RequestParameters>, body: Partial<T>): Observable<T> {
    let url: string = "";

    if (requestParameters.customEndPoint)
      url = requestParameters.customEndPoint;
    else
      url = `${this.url(requestParameters)}${this.getQueryString(requestParameters.queryString)}`;

    return this.httpClient.post<T>(url, body, { headers: requestParameters.headers });
  }

  postWithResponse<TResponse>(requestParameters: Partial<RequestParameters>, body: Partial<any>): Observable<TResponse> {
    let url: string = "";

    if (requestParameters.customEndPoint)
      url = requestParameters.customEndPoint;
    else
      url = `${this.url(requestParameters)}${this.getQueryString(requestParameters.queryString)}`;

    return this.httpClient.post<TResponse>(url, body, { headers: requestParameters.headers });
  }

  put<T>(requestParameters: Partial<RequestParameters>, body: Partial<T>): Observable<T> {
    let url: string = "";

    if (requestParameters.customEndPoint)
      url = requestParameters.customEndPoint;
    else
      url = `${this.url(requestParameters)}${this.getQueryString(requestParameters.queryString)}`;

    return this.httpClient.put<T>(url, body, { headers: requestParameters.headers });
  }

  delete<T>(requestParameters: Partial<RequestParameters>, id: string): Observable<T> {
    let url: string = "";

    if (requestParameters.customEndPoint)
      url = requestParameters.customEndPoint;
    else
      url = `${this.url(requestParameters)}/${id}${this.getQueryString(requestParameters.queryString)}`;

    return this.httpClient.delete<T>(url, { headers: requestParameters.headers });
  }
}


export class RequestParameters {
  controller?: string;
  action?: string;
  queryString?: string;

  headers?: HttpHeaders;
  baseUrl?: string;

  customEndPoint?: string;
}