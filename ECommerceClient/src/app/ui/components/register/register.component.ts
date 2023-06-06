import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { User } from 'src/app/entities/user';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {

  constructor(private register: FormBuilder) { }

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

  onSubmit(formData: User) {
    debugger;;
  }

}

function passwordConfirmValidator(): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {

    let password = (document.getElementById("password") as HTMLInputElement).value

    let passwordConfirm = control.value;
    return password === passwordConfirm ? null : { notSame: true };
  };
}