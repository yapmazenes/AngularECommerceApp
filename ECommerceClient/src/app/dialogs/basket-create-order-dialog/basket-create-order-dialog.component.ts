import { Component, Inject, OnDestroy } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { BaseDialog } from '../base/base-dialog';
declare var $: any;
@Component({
  selector: 'app-basket-create-order-dialog',
  templateUrl: './basket-create-order-dialog.component.html',
  styleUrls: ['./basket-create-order-dialog.component.scss']
})
export class BasketCreateOrderDialogComponent extends BaseDialog<BasketCreateOrderDialogComponent> {
  constructor(dialogRef: MatDialogRef<BasketCreateOrderDialogComponent>, @Inject(MAT_DIALOG_DATA) public data: BasketCreateOrderDialogState) {
    super(dialogRef)
  }

  afterClose() {
    $('#basketModal').modal('show');
  }
}

export enum BasketCreateOrderDialogState {
  Yes,
  No
}
