import { FlatTreeControl } from '@angular/cdk/tree';
import { Component, OnInit } from '@angular/core';
import { MatTreeFlatDataSource, MatTreeFlattener } from '@angular/material/tree';
import { NgxSpinnerService } from 'ngx-spinner';
import { BaseComponent } from 'src/app/base/base.component';
import { AuthorizeMenuDialogComponent } from 'src/app/dialogs/authorize-menu-dialog/authorize-menu-dialog.component';
import { ApplicationConfigurationService } from 'src/app/services/common/application-configuration.service';
import { DialogService } from 'src/app/services/common/dialog.service';

@Component({
  selector: 'app-authorize-menu',
  templateUrl: './authorize-menu.component.html',
  styleUrls: ['./authorize-menu.component.scss']
})
export class AuthorizeMenuComponent extends BaseComponent implements OnInit {

  constructor(spinner: NgxSpinnerService, private applicationService: ApplicationConfigurationService, private dialogService: DialogService) {
    super(spinner);
  }

  async ngOnInit(): Promise<void> {
    this.dataSource.data = (await this.applicationService.getAuthorizeDefinitionEndpoints()).map(m => {
      const treeMenu: TreeMenu = {
        name: m.name,
        actions: m.actions.map(a => {
          const _treeMenu: TreeMenu = {
            name: a.definition,
            code: a.code,
            menuName: m.name
          }
          return _treeMenu;
        })
      };
      return treeMenu;
    });;
  }

  treeControl = new FlatTreeControl<ExampleFlatNode>(
    node => node.level,
    node => node.expandable,
  );

  treeFlattener = new MatTreeFlattener(
    (node: TreeMenu, level: number) => {
      return {
        expandable: !!node.actions && node.actions.length > 0,
        name: node.name,
        level: level,
        code: node.code,
        menuName: node.menuName
      };
    },
    node => node.level,
    node => node.expandable,
    node => node.actions,
  );

  dataSource = new MatTreeFlatDataSource(this.treeControl, this.treeFlattener);

  hasChild = (_: number, node: ExampleFlatNode) => node.expandable;

  assignRole(code: string, name: string, menuName: string) {
    this.dialogService.openDialog({
      componentType: AuthorizeMenuDialogComponent,
      data: { code: code, name: name, menuName: menuName },
      options: {
        width: "750px"
      },
      afterClosed: () => {

      }
    });
  }

}

interface ExampleFlatNode {
  expandable: boolean;
  name: string;
  level: number;
}

interface TreeMenu {
  name?: string,
  actions?: TreeMenu[],
  code?: string,
  menuName?: string
}
