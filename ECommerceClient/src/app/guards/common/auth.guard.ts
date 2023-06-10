import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { _isAuthenticated } from 'src/app/services/common/auth.service';
import { CustomToastrService, ToastrMessageType, ToastrPosition } from 'src/app/services/ui/custom-toastr.service';

export const authGuard: CanActivateFn = (route, state) => {

  //const authService = inject(AuthService);
  const router = inject(Router);
  const toastrService = inject(CustomToastrService);

  if (!_isAuthenticated) {
    router.navigate(["login"], { queryParams: { returnUrl: state.url } });
    toastrService.message("You have to login!", "Unauthorized Access", {
      messageType: ToastrMessageType.Warning,
      position: ToastrPosition.TopRight
    })
  }

  return true;
};
