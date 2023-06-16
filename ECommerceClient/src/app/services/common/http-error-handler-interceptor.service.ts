import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest, HttpStatusCode } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, of } from 'rxjs';
import { CustomToastrService, ToastrMessageType, ToastrPosition } from '../ui/custom-toastr.service';
import { UserService } from './user.service';

@Injectable({
  providedIn: 'root'
})
export class HttpErrorHandlerInterceptorService implements HttpInterceptor {

  constructor(private toastrService: CustomToastrService, private userService: UserService) { }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(req).pipe(catchError(error => {

      switch (error.status) {
        case HttpStatusCode.Unauthorized:
          this.toastrService.message("You are not authorized to perform this action.", "Authorization Information", {
            messageType: ToastrMessageType.Warning,
            position: ToastrPosition.TopRight
          });

          this.userService.refreshTokenLogin(localStorage.getItem("refreshToken")).then(data => {
          });
          break;
        case HttpStatusCode.InternalServerError:
          this.toastrService.message("Server Access Error", "Server Error", {
            messageType: ToastrMessageType.Warning,
            position: ToastrPosition.TopRight
          });
          break;
        case HttpStatusCode.BadRequest:
          this.toastrService.message("Invalid Request", "Invalid Request Error", {
            messageType: ToastrMessageType.Warning,
            position: ToastrPosition.TopRight
          });
          break;
        case HttpStatusCode.NotFound:
          this.toastrService.message("Not Found", "Not Found", {
            messageType: ToastrMessageType.Warning,
            position: ToastrPosition.TopRight
          });
          break;

        default:
          this.toastrService.message("Undefined Server Error", "Undefined Error", {
            messageType: ToastrMessageType.Warning,
            position: ToastrPosition.TopRight
          });
          break;

      }

      return of(error);
    }));
  }
}
