import { Component } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { BaseComponent, SpinnerType } from 'src/app/base/base.component';
import { AlertifyService, MessageType, Position } from 'src/app/services/admin/alertify.service';
import { UserService } from 'src/app/services/common/user.service';

@Component({
  selector: 'app-password-reset',
  templateUrl: './password-reset.component.html',
  styleUrls: ['./password-reset.component.scss']
})
export class PasswordResetComponent extends BaseComponent {
  constructor(spinner: NgxSpinnerService, private userService: UserService, private alertifyService: AlertifyService) {
    super(spinner);
  }

  passwordReset(email: string) {
    this.showSpinner(SpinnerType.BallAtom);
    this.userService.passwordReset(email, () => {
      this.hideSpinner(SpinnerType.BallAtom);
      this.alertifyService.message("Şifre talebi mail adresinize gönderilmiştir.", {
        messageType: MessageType.Notify,
        position: Position.TopRight
      })
      
    });
  }
}
