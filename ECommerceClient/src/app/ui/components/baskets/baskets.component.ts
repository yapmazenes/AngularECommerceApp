import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { BaseComponent, SpinnerType } from 'src/app/base/base.component';
import { ListBasketItem } from 'src/app/contracts/basket/list-basket-item';
import { CreateOrder } from 'src/app/contracts/order/create-order';
import { BasketCreateOrderDialogComponent, BasketCreateOrderDialogState } from 'src/app/dialogs/basket-create-order-dialog/basket-create-order-dialog.component';
import { BasketItemDeleteState, BasketItemRemoveDialogComponent } from 'src/app/dialogs/basket-item-remove-dialog/basket-item-remove-dialog.component';
import { BasketService } from 'src/app/services/common/basket.service';
import { DialogService } from 'src/app/services/common/dialog.service';
import { OrderService } from 'src/app/services/common/order.service';
import { CustomToastrService, ToastrMessageType, ToastrPosition } from 'src/app/services/ui/custom-toastr.service';
declare var $: any;

@Component({
  selector: 'app-baskets',
  templateUrl: './baskets.component.html',
  styleUrls: ['./baskets.component.scss']
})

export class BasketsComponent extends BaseComponent implements OnInit {

  constructor(spinner: NgxSpinnerService,
    private basketService: BasketService, private orderService: OrderService, private toastrService: CustomToastrService,
    private router: Router,
    private dialogService: DialogService) {

    super(spinner);
  }

  basketItems: ListBasketItem[];

  async ngOnInit() {

    this.showSpinner(SpinnerType.BallAtom);
    this.basketItems = await this.basketService.getBasketItems();
    this.hideSpinner(SpinnerType.BallAtom);
  }

  async changeQuantity($event: any) {
    this.showSpinner(SpinnerType.BallAtom);
    const basketItemId = $event.target.closest("tr").attributes["id"].value;
    const quantity = $event.target.value;
    await this.basketService.updateQuantity(basketItemId, quantity);
    this.hideSpinner(SpinnerType.BallAtom);
  }

  async removeBasketItem($event) {
    $('#basketModal').modal('hide');
    this.dialogService.openDialog({
      componentType: BasketItemRemoveDialogComponent,
      data: BasketItemDeleteState.Yes,
      afterClosed: async () => {
        this.showSpinner(SpinnerType.BallAtom);
        const baseItem = $event.target.closest("tr");
        const basketItemId = baseItem.attributes["id"].value;
        await this.basketService.removeBasketItem(basketItemId);
        $(baseItem).fadeOut(500, () => this.hideSpinner(SpinnerType.BallAtom));
        $('#basketModal').modal('show');
      }
    });

  }

  async createOrder() {

    $('#basketModal').modal('hide');
    this.dialogService.openDialog({
      componentType: BasketCreateOrderDialogComponent,
      data: BasketCreateOrderDialogState.Yes,
      afterClosed: async () => {
        this.showSpinner(SpinnerType.BallAtom);

        let order = new CreateOrder();
        order.address = "Adres";
        order.description = "Aciklama";

        await this.orderService.create(order);

        this.hideSpinner(SpinnerType.BallAtom);
        this.toastrService.message("We have received your Order.", "Congratulations, Your Order Created!", {
          messageType: ToastrMessageType.Info,
          position: ToastrPosition.TopRight
        });
        this.router.navigate(["/"]);
        
      }
    });
  }
}
