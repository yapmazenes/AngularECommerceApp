import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';
import { CustomToastrService, ToastrMessageType, ToastrPosition } from 'src/app/services/ui/custom-toastr.service';

export const authGuard: CanActivateFn = (route, state) => {

  const jwtHelper = inject(JwtHelperService);
  const router = inject(Router);
  const toastrService = inject(CustomToastrService);

  const token = localStorage.getItem("accessToken");

  //const decodeToken = jwtHelper.decodeToken(token);
  //const expirationDate = jwtHelper.getTokenExpirationDate(token);
  let isTokenExpired: boolean;

  try {
    isTokenExpired = jwtHelper.isTokenExpired(token);
  } catch (error) {
    isTokenExpired = true;
  }

  if (!token || isTokenExpired) {
    router.navigate(["login"], { queryParams: { returnUrl: state.url } });
    toastrService.message("You have to login!", "Unauthorized Access", {
      messageType: ToastrMessageType.Warning,
      position: ToastrPosition.TopRight
    })
  }

  return true;
};
