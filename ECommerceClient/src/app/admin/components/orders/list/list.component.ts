import { Component, OnInit, ViewChild } from "@angular/core";
import { MatPaginator } from "@angular/material/paginator";
import { MatTableDataSource } from "@angular/material/table";
import { NgxSpinnerService } from "ngx-spinner";
import { BaseComponent, SpinnerType } from "src/app/base/base.component";
import { BasePageModel } from "src/app/contracts/BasePageModel";

import { ListOrder } from "src/app/contracts/order/list-order";
import { OrderDetailDialogComponent } from "src/app/dialogs/order-detail-dialog/order-detail-dialog.component";
import { AlertifyService, MessageType, Position } from "src/app/services/admin/alertify.service";
import { DialogService } from "src/app/services/common/dialog.service";
import { OrderService } from "src/app/services/common/order.service";



@Component({
  selector: 'app-list',
  templateUrl: './list.component.html',
  styleUrls: ['./list.component.scss']
})
export class ListComponent extends BaseComponent implements OnInit {


  constructor(private orderService: OrderService,
    spinner: NgxSpinnerService,
    private alertifyService: AlertifyService,
    private dialogService: DialogService) {
    super(spinner);
  }

  async getOrders() {
    this.showSpinner(SpinnerType.BallAtom);
    let pageIndex = 0;
    let pageSize = 5;

    if (this.paginator != null) {
      pageIndex = this.paginator.pageIndex;
      pageSize = this.paginator.pageSize;
    }

    const orderDatas: BasePageModel<ListOrder[]> = await this.orderService.getAll(pageIndex, pageSize, () => this.hideSpinner(SpinnerType.BallAtom),
      errorMessage => this.alertifyService.message(errorMessage,
        { dismissOthers: true, messageType: MessageType.Error, position: Position.TopRight }));

    this.dataSource = new MatTableDataSource<ListOrder>(orderDatas.datas);
    this.paginator.length = orderDatas.totalCount;
  }

  async ngOnInit() {
    await this.getOrders();
  }

  async pageChanged() {
    await this.getOrders();
  }

  async showDetail(orderId: string) {
    this.dialogService.openDialog({
      componentType: OrderDetailDialogComponent,
      data: orderId,
      options: {
        width: '750px'
      }

    });
  }

  dataSource: MatTableDataSource<ListOrder> = null;
  displayedColumns: string[] = ['orderCode', 'userName', 'totalPrice', 'createdDate', 'completed', 'viewDetail', 'delete'];
  @ViewChild(MatPaginator) paginator: MatPaginator;
}
