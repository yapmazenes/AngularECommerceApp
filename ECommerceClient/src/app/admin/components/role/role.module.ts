import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RoleComponent } from './role.component';
import { RouterModule } from '@angular/router';


import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatTableModule } from '@angular/material/table';


import { MatSidenavModule } from '@angular/material/sidenav';
import { DirectiveModule } from 'src/app/directives/directive/directive.module';
import { MatPaginatorModule } from '@angular/material/paginator';
import { CreateComponent } from '../role/create/create.component';
import { ListComponent } from './list/list.component';
import { MatFormFieldModule } from '@angular/material/form-field';


@NgModule({
  declarations: [
    RoleComponent,
    CreateComponent,
    ListComponent
  ],
  imports: [
    CommonModule,
    RouterModule.forChild([{ path: "", component: RoleComponent }]),
    MatFormFieldModule, MatSidenavModule, MatInputModule, MatButtonModule, MatTableModule, MatPaginatorModule,
    DirectiveModule
  ]
})
export class RoleModule { }
