import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { BaseComponent, SpinnerType } from 'src/app/base/base.component';
import { AlertifyService, MessageType, Position } from 'src/app/services/admin/alertify.service';
import { UserService } from 'src/app/services/common/user.service';

@Component({
  selector: 'app-update-password',
  templateUrl: './update-password.component.html',
  styleUrls: ['./update-password.component.scss']
})
export class UpdatePasswordComponent extends BaseComponent implements OnInit {
  constructor(spinner: NgxSpinnerService,
    private userService: UserService,
    private activatedRoute: ActivatedRoute,
    private alertifyService: AlertifyService,
    private router: Router) {
    super(spinner);
  }

  state: boolean = false;

  ngOnInit(): void {
    this.showSpinner(SpinnerType.BallAtom);
    this.activatedRoute.params.subscribe({
      next: async params => {
        const userId = params["userId"];
        const resetToken = params["resetToken"];

        this.state = await this.userService.verifyResetToken(resetToken, userId, () => {
          this.hideSpinner(SpinnerType.BallAtom);
        });
      }
    });
  }

  updatePassword(password: string, passwordConfirm: string) {
    this.showSpinner(SpinnerType.BallAtom);
    if (password != passwordConfirm) {
      this.hideSpinner(SpinnerType.BallAtom);
      this.alertifyService.message("Şifreleri doğrulayınız!", {
        messageType: MessageType.Error,
        position: Position.TopRight
      });
      return;
    }

    this.activatedRoute.params.subscribe({
      next: async params => {
        const userId = params["userId"];
        const resetToken = params["resetToken"];

        await this.userService.updatePassword(userId, resetToken, password, passwordConfirm,
          () => {
            this.alertifyService.message("Şifre başarıyla güncellenmiştir", {
              messageType: MessageType.Success,
              position: Position.TopRight
            });
            this.router.navigate(["/login"]);
          },
          (error) => {
            console.log(error);
          });
        this.hideSpinner(SpinnerType.BallAtom);
      }
    });

  }

}
