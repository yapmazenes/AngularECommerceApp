import { Component } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { BaseComponent, SpinnerType } from 'src/app/base/base.component';

@Component({
  selector: 'app-customers',
  templateUrl: './customers.component.html',
  styleUrls: ['./customers.component.scss']
})
export class CustomersComponent extends BaseComponent {

  constructor(spinnerService: NgxSpinnerService) {
    super(spinnerService);
  }

  ngOnInit(): void {
    this.showSpinner(SpinnerType.BallAtom);
  }
}
