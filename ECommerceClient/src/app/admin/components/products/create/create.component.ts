import { Component } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { BaseComponent, SpinnerType } from 'src/app/base/base.component';
import { CreateProduct } from 'src/app/contracts/create-product';
import { AlertifyService, MessageType, Position } from 'src/app/services/admin/alertify.service';
import { ProductService } from 'src/app/services/common/product.service';

@Component({
  selector: 'app-create',
  templateUrl: './create.component.html',
  styleUrls: ['./create.component.scss']
})
export class CreateComponent extends BaseComponent {

  constructor(spinner: NgxSpinnerService, private alertify: AlertifyService, private productService: ProductService) {
    super(spinner)
  }

  create(name: HTMLInputElement, stock: HTMLInputElement, price: HTMLInputElement) {
    this.showSpinner(SpinnerType.BallAtom);

    const createProduct: CreateProduct = new CreateProduct
    {
      createProduct.name = name.value;
      createProduct.stock = parseInt(stock.value);
      createProduct.price = parseInt(price.value);
    }

    this.productService.create(createProduct, () => {
      this.hideSpinner(SpinnerType.BallAtom);
      this.alertify.message("Ürün başarıyla eklenmiştir", {
        dismissOthers: true,
        messageType: MessageType.Success,
        position: Position.TopRight
      });
    }
    );
  }
}
