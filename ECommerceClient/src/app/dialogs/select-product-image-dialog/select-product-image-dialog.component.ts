import { Component, Inject, OnInit, Output } from '@angular/core';
import { BaseDialog } from '../base/base-dialog';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { FileUploadOptions } from 'src/app/services/common/file-upload/file-upload.component';
import { ProductService } from 'src/app/services/common/product.service';
import { ListProductImage } from 'src/app/contracts/list-product-image';
import { NgxSpinnerService } from 'ngx-spinner';
import { SpinnerType } from 'src/app/base/base.component';
import { DialogService } from 'src/app/services/common/dialog.service';
import { DeleteDialogComponent, DeleteState } from '../delete-dialog/delete-dialog.component';

@Component({
  selector: 'app-select-product-image-dialog',
  templateUrl: './select-product-image-dialog.component.html',
  styleUrls: ['./select-product-image-dialog.component.scss']
})

export class SelectProductImageDialogComponent extends BaseDialog<SelectProductImageDialogComponent> implements OnInit {

  constructor(
    dialogRef: MatDialogRef<SelectProductImageDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: SelectProductImageState | string,
    private productService: ProductService,
    private spinner: NgxSpinnerService,
    private dialogService: DialogService
  ) {
    super(dialogRef);
  }

  async ngOnInit() {
    this.spinner.show(SpinnerType.BallAtom)
    this.images = await this.productService.getImages(this.data as string, () => {
      this.spinner.hide(SpinnerType.BallAtom);
    });
    console.log(this.images);
  }

  images: ListProductImage[];

  async deleteImage(id: string) {
    this.dialogService.openDialog({
      componentType: DeleteDialogComponent,
      data: DeleteState.Yes,
      afterClosed: async () => {
        this.spinner.show(SpinnerType.BallAtom);
        await this.productService.deleteImage(this.data as string, id, () => {
          this.spinner.hide(SpinnerType.BallAtom);
        });
      }
    });
  }

  changeShowcase(imageId: string) {
    this.spinner.show(SpinnerType.BallAtom);

    this.productService.changeShowcaseImage(imageId, this.data as string, () => {
      this.spinner.hide(SpinnerType.BallAtom);
    });
  }

  @Output() options: Partial<FileUploadOptions> = {
    accept: ".png, .jpg, .jpeg, .gif",
    controller: "products",
    action: "upload",
    explanation: "Ürün resmini seçiniz veya sürükleyiniz.",
    isAdminPage: true,
    queryString: `id=${this.data}`
  }

}

export enum SelectProductImageState {
  Close
}