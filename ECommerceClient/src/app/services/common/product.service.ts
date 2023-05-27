import { Injectable } from '@angular/core';
import { HttpClientService } from './http-client.service';
import { CreateProduct } from 'src/app/contracts/create-product';
import { HttpErrorResponse } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class ProductService {

  constructor(private httpClientService: HttpClientService) { }

  create(createProduct: CreateProduct, succesCallback?: any, errorCallback?: any) {
    this.httpClientService.post({ controller: 'products' }, createProduct)
      .subscribe({
        next: (result) => {
          succesCallback();
        },
        error: (errorResponse: HttpErrorResponse) => {
          const _errors: Array<{ key: string, value: Array<string> }> = errorResponse.error;
          let message = "";
          _errors.forEach((v, index) => {
            v.value.forEach((errorMessage, index) => {
              message = `${errorMessage}<br>`;
            });
          });
          errorCallback(message);
        }
      });
  }
}
