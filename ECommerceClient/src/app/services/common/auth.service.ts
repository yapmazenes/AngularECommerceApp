import { Injectable } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private jwtHelper: JwtHelperService) { }

  identityCheck() {
    const token = localStorage.getItem("accessToken");

    //const decodeToken = jwtHelper.decodeToken(token);
    //const expirationDate = jwtHelper.getTokenExpirationDate(token);
    let isTokenExpired: boolean;

    try {
      isTokenExpired = this.jwtHelper.isTokenExpired(token);
    } catch (error) {
      isTokenExpired = true;
    }

    _isAuthenticated = token != null && !isTokenExpired;
  }

  get isAuthenticated(): boolean {
    return _isAuthenticated;
  }

}

export let _isAuthenticated: boolean;