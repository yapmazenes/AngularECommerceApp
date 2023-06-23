import { Injectable } from '@angular/core';
import { HttpClientService } from './http-client.service';
import { CreateOrder } from 'src/app/contracts/order/create-order';
import { lastValueFrom } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class OrderService {

  constructor(private httpClientService: HttpClientService) { }

  async create(order: CreateOrder) {
    const observable = this.httpClientService.post({
      controller: "orders"
    }, order);

    await lastValueFrom(observable);
  }
}
