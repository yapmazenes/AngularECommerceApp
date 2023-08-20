import { Component, Inject, OnInit } from '@angular/core';
import { BaseDialog } from '../base/base-dialog';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { OrderService } from 'src/app/services/common/order.service';
import { OrderDetail } from 'src/app/contracts/order/order-detail';
import { DialogService } from 'src/app/services/common/dialog.service';
import { CompleteOrderDialogComponent, CompleteOrderState } from '../complete-order-dialog/complete-order-dialog.component';
import { NgxSpinnerService } from 'ngx-spinner';
import { SpinnerType } from 'src/app/base/base.component';
import { CustomToastrService, ToastrMessageType, ToastrPosition } from 'src/app/services/ui/custom-toastr.service';

@Component({
  selector: 'app-order-detail-dialog',
  templateUrl: './order-detail-dialog.component.html',
  styleUrls: ['./order-detail-dialog.component.scss']
})
export class OrderDetailDialogComponent extends BaseDialog<OrderDetailDialogComponent> implements OnInit {
  constructor(dialogRef: MatDialogRef<OrderDetailDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: OrderDetailDialogState | string,
    private orderService: OrderService,
    private dialogService: DialogService,
    private spinner: NgxSpinnerService,
    private toastrService: CustomToastrService) {
    super(dialogRef);
  }

  orderDetail: OrderDetail;
  orderDetailObject;
  displayedColumns: string[] = ['name', 'price', 'quantity', 'totalPrice'];
  dataSource;
  clickedRows = new Set<any>();
  totalPrice: number;

  async ngOnInit(): Promise<void> {

    this.orderDetail = await this.orderService.getOrderById(this.data as string, () => { }, (message) => { console.log(message) });

    this.dataSource = this.orderDetail.basketItems;

    this.totalPrice = this.dataSource
      .map((basketItem, index) => basketItem.price * basketItem.quantity)
      .reduce((price, current) => price + current);

  }

  completeOrder() {
    this.dialogService.openDialog({
      componentType: CompleteOrderDialogComponent,
      data: CompleteOrderState.Yes,
      afterClosed: async () => {
        this.spinner.show(SpinnerType.BallAtom);
        var result = await this.orderService.completeOrder(this.data as string);
        this.spinner.hide(SpinnerType.BallAtom);
        if (result?.status == true)
          this.toastrService.message("Sipariş başarıyla alınmıştır", "Sipariş", { messageType: ToastrMessageType.Success, position: ToastrPosition.TopRight });
        else
          this.toastrService.message("Sipariş alınırken bir hata oluştu", "Sipariş", { messageType: ToastrMessageType.Error, position: ToastrPosition.TopRight });
      }
    })
  }

}

export enum OrderDetailDialogState {
  Close,
  OrderComplete
}
