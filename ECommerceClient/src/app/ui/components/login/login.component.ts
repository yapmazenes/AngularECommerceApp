import { Component, ElementRef, HostListener, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { BaseComponent, SpinnerType } from 'src/app/base/base.component';
import { AuthService } from 'src/app/services/common/auth.service';
import { UserService } from 'src/app/services/common/user.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent extends BaseComponent {

  constructor(private userService: UserService, spinner: NgxSpinnerService, private authService: AuthService,
    private activatedRoute: ActivatedRoute, private router: Router) {
    super(spinner);
  }

  @ViewChild('loginButton') loginButton: ElementRef<HTMLElement>;

  async login(userNameOrEmail: string, password: string) {

    this.showSpinner(SpinnerType.BallAtom);
    await this.userService.login(userNameOrEmail, password, () => {

      this.authService.identityCheck();

      this.activatedRoute.queryParams.subscribe(queryParams => {
        const returnUrl = queryParams["returnUrl"];
        if (returnUrl)
          this.router.navigate([returnUrl]);
        this.router.navigate([""]);
      });

      this.hideSpinner(SpinnerType.BallAtom);

    });
  }

  @HostListener('document:keyup', ['$event'])
  handleEnterKeyboardEvent(event: KeyboardEvent) {
    if (event.key === 'Enter') {
      this.loginButton.nativeElement.click();
    }
  }
}
