import { Injectable } from '@angular/core';
import { HttpClientService } from './http-client.service';
import { CreateOrder } from 'src/app/contracts/order/create-order';
import { lastValueFrom } from 'rxjs';
import { ListOrder } from 'src/app/contracts/order/list-order';
import { BasePageModel } from 'src/app/contracts/BasePageModel';
import { HttpErrorResponse } from '@angular/common/http';
import { OrderDetail } from 'src/app/contracts/order/order-detail';

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

  async getAll(page: number = 0, size: number = 5, successCallback?: () => void, errorCallback?: (message: string) => void): Promise<BasePageModel<ListOrder[]>> {
    const observableData = this.httpClientService.get<BasePageModel<ListOrder[]>>({
      controller: "orders",
      queryString: `page=${page}&size=${size}`
    });

    var promiseValue = lastValueFrom(observableData);

    promiseValue.then(d => successCallback())
      .catch((error: HttpErrorResponse) => errorCallback(error.message));

    return await promiseValue;
  }

  async getOrderById(orderId: string, successCallback?: () => void, errorCallback?: (message: string) => void): Promise<OrderDetail> {
    const observableData = this.httpClientService.get<OrderDetail>({
      controller: "orders",
    }, orderId);

    var promiseValue = lastValueFrom(observableData);

    promiseValue.then(d => successCallback())
      .catch((error: HttpErrorResponse) => errorCallback(error.message));

    return await promiseValue;
  }
}
