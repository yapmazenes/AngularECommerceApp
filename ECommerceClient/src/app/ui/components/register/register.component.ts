import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';

import { User } from 'src/app/entities/user';
import { UserService } from 'src/app/services/common/user.service';
import { CustomToastrService, ToastrMessageType, ToastrPosition } from 'src/app/services/ui/custom-toastr.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {

  constructor(private register: FormBuilder, private userService: UserService, private toastrService: CustomToastrService) { }

  form: FormGroup;
  ngOnInit(): void {
    this.form = this.register.group({
      nameSurname: ["", [
        Validators.required, Validators.maxLength(50), Validators.minLength(3)
      ]],
      userName: ["", [
        Validators.required, Validators.maxLength(50), Validators.minLength(3)
      ]],
      email: ["", [
        Validators.required, Validators.maxLength(250), Validators.email
      ]],
      password: ["", [
        Validators.required, Validators.maxLength(50), Validators.minLength(3)
      ]],
      passwordConfirm: ["", [passwordConfirmValidator()]]

    });
  }



  get component() {
    return this.form.controls;
  }

  async onSubmit(user: User) {
    if (this.form.invalid) return;

    const result = await this.userService.create(user);

    if (result.succeded) {
      this.toastrService.message(result.message, "User Register Success",
        {
          messageType: ToastrMessageType.Success,
          position: ToastrPosition.TopRight
        });
    } else {
      this.toastrService.message(result.message, "User Register Failed",
        {
          messageType: ToastrMessageType.Error,
          position: ToastrPosition.TopRight
        });
    }

  }

}

function passwordConfirmValidator(): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {

    let password = (document.getElementById("password") as HTMLInputElement).value

    let passwordConfirm = control.value;
    return password === passwordConfirm ? null : { notSame: true };
  };
}