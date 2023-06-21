import { Component, OnInit } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { BaseComponent, SpinnerType } from 'src/app/base/base.component';
import { ListBasketItem } from 'src/app/contracts/basket/list-basket-item';
import { BasketService } from 'src/app/services/common/basket.service';
declare var $: any;

@Component({
  selector: 'app-baskets',
  templateUrl: './baskets.component.html',
  styleUrls: ['./baskets.component.scss']
})

export class BasketsComponent extends BaseComponent implements OnInit {

  constructor(spinner: NgxSpinnerService, private basketService: BasketService) {
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
    this.showSpinner(SpinnerType.BallAtom);
    const baseItem = $event.target.closest("tr");
    const basketItemId = baseItem.attributes["id"].value;
    this.basketService.removeBasketItem(basketItemId);
    $(baseItem).fadeOut(500, () => this.hideSpinner(SpinnerType.BallAtom));
  }

}
