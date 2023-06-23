import { Component, Inject, OnInit } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { BaseComponent, SpinnerType } from 'src/app/base/base.component';
import { HubUrls } from 'src/app/constants/hub-urls';
import { ReceiveFunctions } from 'src/app/constants/receive-functions';
import { SignalRService } from 'src/app/services/common/signalr.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent extends BaseComponent implements OnInit {

  constructor(spinnerService: NgxSpinnerService,
    @Inject("baseHubUrl") private baseHubUrl: string,
    private signalRService: SignalRService) {

    super(spinnerService);
    signalRService.start(`http://${baseHubUrl}${HubUrls.ProductHub}`);
    signalRService.start(`http://${baseHubUrl}${HubUrls.OrderHub}`);
  }

  ngOnInit(): void {
    this.showSpinner(SpinnerType.BallAtom);
    this.signalRService.on(ReceiveFunctions.ProductAddedMessageReceiveFunction, message => {
      alert(message)
    });
    this.signalRService.on(ReceiveFunctions.OrderAddedMessageReceiveFunction, message => {
      alert(message)
    });

  }
}
