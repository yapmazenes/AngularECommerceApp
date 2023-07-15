import { Component, Inject, OnInit } from '@angular/core';
import { BaseDialog } from '../base/base-dialog';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { OrderService } from 'src/app/services/common/order.service';
import { OrderDetail } from 'src/app/contracts/order/order-detail';

@Component({
  selector: 'app-order-detail-dialog',
  templateUrl: './order-detail-dialog.component.html',
  styleUrls: ['./order-detail-dialog.component.scss']
})
export class OrderDetailDialogComponent extends BaseDialog<OrderDetailDialogComponent> implements OnInit {
  constructor(dialogRef: MatDialogRef<OrderDetailDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: OrderDetailDialogState | string,
    private orderService: OrderService) {
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

}

export enum OrderDetailDialogState {
  Close,
  OrderComplete
}
