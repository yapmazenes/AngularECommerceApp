import { Injectable } from '@angular/core';
import { HttpClientService } from './http-client.service';
import { User } from 'src/app/entities/user';
import { CreateUser } from 'src/app/contracts/users/create-user';
import { lastValueFrom } from 'rxjs';
import { Token } from 'src/app/contracts/login/token';
import { CustomToastrService, ToastrMessageType, ToastrPosition } from '../ui/custom-toastr.service';
import { LoginResponse } from 'src/app/contracts/login/login-response';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private httpClientService: HttpClientService, private toastrService: CustomToastrService) { }

  async create(user: User): Promise<CreateUser> {
    const observable = this.httpClientService.post<CreateUser | User>({
      controller: "users"
    }, user);

    return await lastValueFrom(observable) as CreateUser;
  }

  async login(userNameOrEmail: string, password: string, callBackFunction?: () => void): Promise<any> {

    const observable = this.httpClientService.post<any | LoginResponse>({
      controller: "authentication",
      action: "login"

    }, { userNameOrEmail: userNameOrEmail, password: password })

    const loginResponse = await lastValueFrom(observable) as LoginResponse;
    if (loginResponse) {
      localStorage.setItem("accessToken", loginResponse.token.accessToken);

      this.toastrService.message("User login successfully.", "Login Successfully", {
        messageType: ToastrMessageType.Success,
        position: ToastrPosition.TopRight

      });

    }

    callBackFunction();
  }
}
