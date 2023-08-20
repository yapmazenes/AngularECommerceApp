
import { Component, OnInit, ViewChild } from "@angular/core";
import { MatPaginator } from "@angular/material/paginator";
import { MatTableDataSource } from "@angular/material/table";
import { NgxSpinnerService } from "ngx-spinner";
import { BaseComponent, SpinnerType } from "src/app/base/base.component";
import { BasePageModel } from "src/app/contracts/BasePageModel";

import { ListUser } from "src/app/contracts/users/list-user";
import { AuthorizeUserDialogComponent } from "src/app/dialogs/authorize-user-dialog/authorize-user-dialog.component";
import { OrderDetailDialogComponent } from "src/app/dialogs/order-detail-dialog/order-detail-dialog.component";
import { AlertifyService, MessageType, Position } from "src/app/services/admin/alertify.service";
import { DialogService } from "src/app/services/common/dialog.service";
import { UserService } from "src/app/services/common/user.service";
@Component({
  selector: 'app-list',
  templateUrl: './list.component.html',
  styleUrls: ['./list.component.scss']
})
export class ListComponent extends BaseComponent implements OnInit {

  constructor(private userService: UserService,
    spinner: NgxSpinnerService,
    private alertifyService: AlertifyService,
    private dialogService: DialogService) {
    super(spinner);
  }

  async getUsers() {
    this.showSpinner(SpinnerType.BallAtom);
    let pageIndex = 0;
    let pageSize = 5;

    if (this.paginator != null) {
      pageIndex = this.paginator.pageIndex;
      pageSize = this.paginator.pageSize;
    }

    const userDatas: BasePageModel<ListUser[]> = await this.userService.getAll(pageIndex, pageSize, () => this.hideSpinner(SpinnerType.BallAtom),
      errorMessage => this.alertifyService.message(errorMessage,
        { dismissOthers: true, messageType: MessageType.Error, position: Position.TopRight }));

    this.dataSource = new MatTableDataSource<ListUser>(userDatas.datas);
    this.paginator.length = userDatas.totalCount;
  }

  async ngOnInit() {
    await this.getUsers();
  }

  async pageChanged() {
    await this.getUsers();
  }

  async assignRole(userId: string) {
    this.dialogService.openDialog({
      componentType: AuthorizeUserDialogComponent, data: userId, options: { width: "750px" },
      afterClosed: () => {
        this.alertifyService.message("Roles has been setted successfully.", { messageType: MessageType.Success, position: Position.TopRight });
      }
    })
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

  dataSource: MatTableDataSource<ListUser> = null;
  displayedColumns: string[] = ['userName', 'nameSurname', 'email', 'twoFactorEnabled', "role", 'delete'];
  @ViewChild(MatPaginator) paginator: MatPaginator;
}