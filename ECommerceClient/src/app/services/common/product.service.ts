import { Injectable } from '@angular/core';
import { HttpClientService } from './http-client.service';
import { CreateProduct } from 'src/app/contracts/create-product';

@Injectable({
  providedIn: 'root'
})
export class ProductService {

  constructor(private httpClientService: HttpClientService) { }

  create(createProduct: CreateProduct, succesCallback?: any) {
    this.httpClientService.post({ controller: 'products' }, createProduct)
      .subscribe((result) => {
        succesCallback();
      });
  }
}
