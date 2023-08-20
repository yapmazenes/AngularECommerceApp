import { Injectable } from '@angular/core';
import { HttpClientService } from './http-client.service';
import { Observable, lastValueFrom } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class RoleService {

  constructor(private httpClientService: HttpClientService) { }

  async getRoles(page: number, size: number, succesCallback?: () => void, errorCallback?: (message: string) => void) {  
    const observable: Observable<any> = this.httpClientService.get({ controller: "roles", queryString: `page=${page}&size=${size}` },);
    const promiseData = lastValueFrom(observable);
    promiseData.then(succesCallback).catch(errorCallback);
    return await promiseData;
  }

  async create(name: string, succesCallback?: () => void, errorCallback?: (message: string) => void) {
    const observable: Observable<any> = this.httpClientService.post({ controller: "roles" }, { name: name });
    succesCallback();
    const promiseData = lastValueFrom(observable);
    promiseData.then(succesCallback).catch(errorCallback);
    return await promiseData as { succeeded: boolean };
  }
}
