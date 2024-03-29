import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { BaseDialog } from '../base/base-dialog';
import { NgxSpinnerService } from 'ngx-spinner';
import { SpinnerType } from 'src/app/base/base.component';
import { RoleService } from 'src/app/services/common/role.service';

import { MatSelectionList } from '@angular/material/list';
import { ListUser } from 'src/app/contracts/users/list-user';
import { UserService } from 'src/app/services/common/user.service';

declare var $: any;

@Component({
  selector: 'app-authorize-user-dialog',
  templateUrl: './authorize-user-dialog.component.html',
  styleUrls: ['./authorize-user-dialog.component.scss']
})
export class AuthorizeUserDialogComponent extends BaseDialog<AuthorizeUserDialogComponent> implements OnInit {
  constructor(dialogRef: MatDialogRef<AuthorizeUserDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private roleService: RoleService,
    private userService: UserService,
    private spinner: NgxSpinnerService) {
    super(dialogRef)
  }
  roles: { datas: ListUser[], totalCount: number };
  assignedRoles: Array<string>;
  listRoles: { name: string, selected: boolean }[];

  async ngOnInit() {
    this.spinner.show(SpinnerType.BallAtom);
    this.assignedRoles = await this.userService.getRolesToUser(this.data, () => this.spinner.hide(SpinnerType.BallAtom));

    this.roles = await this.roleService.getRoles(-1, -1);

    this.listRoles = this.roles.datas.map((r: any) => {
      return {
        name: r.name,
        selected: this.assignedRoles?.indexOf(r.name) > -1
      }
    });
  }

  assignRoles(rolesComponent: MatSelectionList) {
    const roles: string[] = rolesComponent.selectedOptions.selected.map(o => o._elementRef.nativeElement.innerText);
    this.spinner.show(SpinnerType.BallAtom);
    this.userService.assignRoleToUser(this.data, roles,
      () => {
        this.spinner.hide(SpinnerType.BallAtom);
      }, error => {

      })
  }
}
