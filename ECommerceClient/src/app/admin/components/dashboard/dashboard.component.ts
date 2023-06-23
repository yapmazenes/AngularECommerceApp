import { Component, Inject, OnDestroy, OnInit } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { BaseComponent, SpinnerType } from 'src/app/base/base.component';
import { HubUrls } from 'src/app/constants/hub-urls';
import { ReceiveFunctions } from 'src/app/constants/receive-functions';
import { AlertifyService, MessageType, Position } from 'src/app/services/admin/alertify.service';
import { SignalRService } from 'src/app/services/common/signalr.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent extends BaseComponent implements OnInit, OnDestroy {

  constructor(spinnerService: NgxSpinnerService,
    @Inject("baseHubUrl") private baseHubUrl: string,
    private signalRService: SignalRService,
    private alertify: AlertifyService
  ) {

    super(spinnerService);
    
    signalRService.start(baseHubUrl, HubUrls.ProductHub);
    signalRService.start(baseHubUrl, HubUrls.OrderHub);
  }
  ngOnDestroy(): void {
    
  }

  ngOnInit(): void {
    this.showSpinner(SpinnerType.BallAtom);
    this.signalRService.on(HubUrls.ProductHub, ReceiveFunctions.ProductAddedMessageReceiveFunction, message => {
      this.alertify.message(message, {
        messageType: MessageType.Notify,
        position: Position.TopRight
      });
    });
    this.signalRService.on(HubUrls.OrderHub, ReceiveFunctions.OrderAddedMessageReceiveFunction, message => {
      this.alertify.message(message, {
        messageType: MessageType.Notify,
        position: Position.TopRight
      });
    });

  }
}
