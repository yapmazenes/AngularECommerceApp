import { Injectable } from '@angular/core';
import { HttpClientService } from './http-client.service';
import { MenuDto } from 'src/app/contracts/application-configuration/menuDto';
import { Observable, lastValueFrom } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ApplicationConfigurationService {

  constructor(private httpClientService: HttpClientService) { }

  async getAuthorizeDefinitionEndpoints() {
    const observable = this.httpClientService.get<MenuDto[]>({
      controller: "ApplicationConfiguration",
    });

    return await lastValueFrom(observable);
  }
}
