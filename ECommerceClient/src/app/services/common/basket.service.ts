import { Injectable } from '@angular/core';
import { HttpClientService } from './http-client.service';
import { lastValueFrom } from 'rxjs';
import { ListBasketItem } from 'src/app/contracts/basket/list-basket-item';
import { CreateBasketItem } from 'src/app/contracts/basket/create-basket-item';

@Injectable({
  providedIn: 'root'
})
export class BasketService {

  constructor(private httpClientService: HttpClientService) { }

  async getBasketItems(): Promise<ListBasketItem[]> {

    const observable = this.httpClientService.get<ListBasketItem[]>({
      controller: "baskets"
    });

    return await lastValueFrom(observable);
  }

  async addItemToBasket(basketItem: CreateBasketItem): Promise<void> {
    const observable = this.httpClientService.post({
      controller: "baskets"
    }, basketItem);

    await lastValueFrom(observable);
  }

  async updateQuantity(basketItemId: string, quantity: number) {
    const observable = this.httpClientService.put({
      controller: "baskets"
    }, {
      basketItemId: basketItemId,
      quantity: quantity
    });

    await lastValueFrom(observable);
  }

  async removeBasketItem(basketItemId: string) {
    const observable = this.httpClientService.delete({
      controller: "baskets"
    }, basketItemId);

    await lastValueFrom(observable);
  }
}
