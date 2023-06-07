import { Injectable } from '@angular/core';
import { HttpClientService } from './http-client.service';
import { User } from 'src/app/entities/user';
import { CreateUser } from 'src/app/contracts/users/create-user';
import { lastValueFrom } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private httpClientService: HttpClientService) { }

  async create(user: User): Promise<CreateUser> {
    const observable = this.httpClientService.post<CreateUser | User>({
      controller: "users"
    }, user);

    return await lastValueFrom(observable) as CreateUser;
  }
}
