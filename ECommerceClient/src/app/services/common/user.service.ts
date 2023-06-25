import { Injectable } from '@angular/core';
import { HttpClientService } from './http-client.service';
import { User } from 'src/app/entities/user';
import { CreateUser } from 'src/app/contracts/users/create-user';
import { Observable, lastValueFrom } from 'rxjs';
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

  processLogin(loginResponse: LoginResponse, showNotify: boolean) {
    localStorage.setItem("accessToken", loginResponse.token.accessToken);
    localStorage.setItem("refreshToken", loginResponse.token.refreshToken);

    if (showNotify) {
      this.toastrService.message("User login successfully.", "Login Successfully", {
        messageType: ToastrMessageType.Success,
        position: ToastrPosition.TopRight
      });
    }
  }

  async login(userNameOrEmail: string, password: string, callBackFunction?: () => void): Promise<any> {

    const observable = this.httpClientService.post<any | LoginResponse>({
      controller: "authentication",
      action: "login"

    }, { userNameOrEmail: userNameOrEmail, password: password })

    const loginResponse = await lastValueFrom(observable) as LoginResponse;

    if (loginResponse)
      this.processLogin(loginResponse, true);

    callBackFunction();
  }

  async refreshTokenLogin(refreshToken: string, callBackFunction?: (state) => void): Promise<any> {
    const observable: Observable<any | LoginResponse> = this.httpClientService.post({
      action: "refreshTokenLogin",
      controller: "authentication",
    }, { RefreshToken: refreshToken });

    try {
      const loginResponse = await lastValueFrom(observable) as LoginResponse;

      if (loginResponse)
        this.processLogin(loginResponse, false);

      callBackFunction(loginResponse ? true : false);

    } catch {

      callBackFunction(false);
    }
  }
}

