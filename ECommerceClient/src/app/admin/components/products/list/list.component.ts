import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, } from '@angular/material/paginator';
import { MatTableDataSource, } from '@angular/material/table';
import { NgxSpinnerService } from 'ngx-spinner';
import { BaseComponent, SpinnerType } from 'src/app/base/base.component';
import { BasePageModel } from 'src/app/contracts/BasePageModel';
import { ListProduct } from 'src/app/contracts/list-product';
import { SelectProductImageDialogComponent } from 'src/app/dialogs/select-product-image-dialog/select-product-image-dialog.component';
import { AlertifyService, MessageType, Position } from 'src/app/services/admin/alertify.service';
import { DialogService } from 'src/app/services/common/dialog.service';
import { ProductService } from 'src/app/services/common/product.service';

@Component({
  selector: 'app-list',
  templateUrl: './list.component.html',
  styleUrls: ['./list.component.scss']
})
export class ListComponent extends BaseComponent implements OnInit {


  constructor(private productService: ProductService, spinner: NgxSpinnerService, private alertifyService: AlertifyService, private dialogService: DialogService) {
    super(spinner);
  }

  async getProducts() {
    this.showSpinner(SpinnerType.BallAtom);
    let pageIndex = 0;
    let pageSize = 5;

    if (this.paginator != null) {
      pageIndex = this.paginator.pageIndex;
      pageSize = this.paginator.pageSize;
    }

    const allProducts: BasePageModel<ListProduct[]> = await this.productService.getAll(pageIndex, pageSize, () => this.hideSpinner(SpinnerType.BallAtom),
      errorMessage => this.alertifyService.message(errorMessage,
        { dismissOthers: true, messageType: MessageType.Error, position: Position.TopRight }));
    this.dataSource = new MatTableDataSource<ListProduct>(allProducts.datas);
    this.paginator.length = allProducts.totalCount;
  }

  async ngOnInit() {
    await this.getProducts();
  }

  async pageChanged() {
    await this.getProducts();
  }

  addProductImages(productId: string) {
    this.dialogService.openDialog({
      componentType: SelectProductImageDialogComponent,
      data: productId,
      options: {
        width: "1400px"
      }

    })

  }

  dataSource: MatTableDataSource<ListProduct> = null;
  displayedColumns: string[] = ['name', 'stock', 'price', 'createdDate', 'updatedDate', 'photos', 'edit', 'delete'];
  @ViewChild(MatPaginator) paginator: MatPaginator;
}
