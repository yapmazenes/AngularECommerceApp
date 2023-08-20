import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, } from '@angular/material/paginator';
import { MatTableDataSource, } from '@angular/material/table';
import { NgxSpinnerService } from 'ngx-spinner';
import { BaseComponent, SpinnerType } from 'src/app/base/base.component';
import { BasePageModel } from 'src/app/contracts/BasePageModel';
import { ListRole } from 'src/app/contracts/role/list-role';
import { AlertifyService, MessageType, Position } from 'src/app/services/admin/alertify.service';
import { DialogService } from 'src/app/services/common/dialog.service';
import { RoleService } from 'src/app/services/common/role.service';

@Component({
  selector: 'app-list',
  templateUrl: './list.component.html',
  styleUrls: ['./list.component.scss']
})
export class ListComponent extends BaseComponent implements OnInit {


  constructor(
    private roleService: RoleService, spinner: NgxSpinnerService,
    private alertifyService: AlertifyService,
    private dialogService: DialogService) {

    super(spinner);
  }

  async getRoles() {
    this.showSpinner(SpinnerType.BallAtom);
    let pageIndex = 0;
    let pageSize = 5;

    if (this.paginator != null) {
      pageIndex = this.paginator.pageIndex;
      pageSize = this.paginator.pageSize;
    }

    const allRoles: BasePageModel<ListRole[]> = await this.roleService.getRoles(pageIndex, pageSize, () => this.hideSpinner(SpinnerType.BallAtom),
      errorMessage => this.alertifyService.message(errorMessage,
        { dismissOthers: true, messageType: MessageType.Error, position: Position.TopRight }));
    this.dataSource = new MatTableDataSource<ListRole>(allRoles.datas);
    this.paginator.length = allRoles.totalCount;
  }

  async ngOnInit() {
    await this.getRoles();
  }

  async pageChanged() {
    await this.getRoles();
  }

  dataSource: MatTableDataSource<ListRole> = null;
  displayedColumns: string[] = ['name', 'edit', 'delete'];
  @ViewChild(MatPaginator) paginator: MatPaginator;
}
