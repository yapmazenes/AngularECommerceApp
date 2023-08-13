import { Injectable } from '@angular/core';
import { HttpClientService } from './http-client.service';
import { Observable, lastValueFrom } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthorizationEndpointService {

  constructor(private httpClientService: HttpClientService) { }

  async getRolesToEndpoint(code: string, menuName: string, successCallBack?: () => void, errorCallBack?: (error) => void) {
    const observable = this.httpClientService.postWithResponse<any>({
      controller: "AuthorizationEndpoints",
      action: "GetRolesToEndpoint"
    }, {
      Code: code,
      Menu: menuName
    });

    const promiseData = lastValueFrom(observable);

    promiseData.then(successCallBack).catch(errorCallBack);

    return (await promiseData)?.roles;
  }

  async assignRoleEndpoint(roles: string[], code: string, menu: string, successCallBack?: () => void, errorCallBack?: (error) => void) {
    const observable = this.httpClientService.post({
      controller: "AuthorizationEndpoints",
      action: "AssignRoleEndpoint"
    }, {
      roles: roles,
      endpointCode: code,
      menu: menu
    });

    const promiseData = observable.subscribe({
      next: successCallBack,
      error: errorCallBack
    });

    await promiseData;

  }
}
