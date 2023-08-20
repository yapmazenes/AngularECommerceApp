import { Injectable } from '@angular/core';
import { HttpClientService } from './http-client.service';
import { User } from 'src/app/entities/user';
import { CreateUser } from 'src/app/contracts/users/create-user';
import { Observable, lastValueFrom } from 'rxjs';
import { Token } from 'src/app/contracts/login/token';
import { CustomToastrService, ToastrMessageType, ToastrPosition } from '../ui/custom-toastr.service';
import { LoginResponse } from 'src/app/contracts/login/login-response';
import { ListUser } from '../../contracts/users/list-user';
import { BasePageModel } from '../../contracts/BasePageModel';
import { HttpErrorResponse } from '@angular/common/http';

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

  async getAll(page: number = 0, size: number = 5, successCallBack: () => void, errorCallBack: (errorMessage: any) => void) {
    const observableData = this.httpClientService.get<BasePageModel<ListUser[]>>({
      controller: "users",
      queryString: `page=${page}&size=${size}`
    });

    var promiseValue = lastValueFrom(observableData);

    promiseValue.then(d => successCallBack())
      .catch((error: HttpErrorResponse) => errorCallBack(error.message));

    return await promiseValue;
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

  async passwordReset(email: string, callBackFunction?: () => void) {
    const observable = this.httpClientService.post({
      controller: "Authentication",
      action: "password-reset"
    }, { email: email });

    await lastValueFrom(observable);
    callBackFunction();
  }

  async verifyResetToken(resetToken: string, userId: string, callBackFunction?: () => void): Promise<boolean> {
    const observable: any = this.httpClientService.post({
      controller: "Authentication",
      action: "verify-reset-token"
    }, { resetToken: resetToken, userId: userId });

    const response: { state: boolean } = await lastValueFrom(observable);

    callBackFunction();

    return response.state;
  }

  async updatePassword(userId: string, resetToken: string, password: string, passwordConfirm: string,
    successCallBack?: () => void,
    errorCallBack?: (error) => void) {
    const observable: Observable<any> = this.httpClientService.post({
      controller: "users",
      action: "update-password"
    }, {
      userId: userId,
      resetToken: resetToken,
      password: password,
      passwordConfirm: passwordConfirm
    });

    const promiseData: Promise<any> = lastValueFrom(observable);

    promiseData.then(value => successCallBack()).catch(error => errorCallBack(error));
    await promiseData;
  }

  async assignRoleToUser(userId: string, roles: string[], successCallBack?: () => void, errorCallBack?: (error) => void) {
    const observable = this.httpClientService.post({
      controller: "Users",
      action: "assign-role-to-user"
    }, {
      roles: roles,
      userId: userId
    });

    const promiseData = observable.subscribe({
      next: successCallBack,
      error: errorCallBack
    });

    await promiseData;

  }

  async getRolesToUser(userId: string, successCallBack?: () => void, errorCallBack?: (error) => void) {
    const observable = this.httpClientService.get<{ roles: string[] }>({
      controller: "Users",
      action: "get-roles-to-user"
    }, userId);

    const promiseData = lastValueFrom(observable);

    promiseData.then(successCallBack).catch(errorCallBack);

    return (await promiseData)?.roles;
  }

}

