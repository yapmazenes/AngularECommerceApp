import { Injectable } from '@angular/core';
import { HttpClientService } from './http-client.service';
import { CreateProduct } from 'src/app/contracts/create-product';
import { HttpErrorResponse } from '@angular/common/http';
import { ListProduct } from 'src/app/contracts/list-product';
import { Observable, lastValueFrom } from 'rxjs';
import { BasePageModel } from 'src/app/contracts/BasePageModel';
import { ListProductImage } from 'src/app/contracts/list-product-image';

@Injectable({
  providedIn: 'root'
})
export class ProductService {

  constructor(private httpClientService: HttpClientService) { }

  create(createProduct: CreateProduct, succesCallback?: () => void, errorCallback?: (message: string) => void) {
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

  async getAll(page: number = 0, size: number = 5, successCallback?: () => void, errorCallback?: (message: string) => void): Promise<BasePageModel<ListProduct[]>> {
    const observableData = this.httpClientService.get<BasePageModel<ListProduct[]>>(
      {
        controller: "products",
        queryString: `page=${page}&size=${size}`
      });
    var promiseValue = lastValueFrom(observableData);

    promiseValue.then(d => successCallback())
      .catch((error: HttpErrorResponse) => errorCallback(error.message));

    return await promiseValue;
  }

  async delete(id: string) {

    const deleteObservable = this.httpClientService.delete({ controller: "products" }, id);

    await lastValueFrom(deleteObservable);
  }

  async getImages(id: string, successCallback: () => void): Promise<ListProductImage[]> {
    const getObservable: Observable<ListProductImage[]> = this.httpClientService.get<ListProductImage[]>({
      action: "getProductImages",
      controller: "products"
    }, id);

    const images = await lastValueFrom(getObservable);
    successCallback();
    return images;
  }

  async deleteImage(productId: string, imageId: string, successCallback: () => void) {
    this.httpClientService.delete<any>({
      controller: "products",
      action: "deleteProductImage",
      queryString: `imageId=${imageId}`
    }, productId).subscribe(() => {

      successCallback();
    });
  }

  async changeShowcaseImage(imageId: string, productId: string, successCallBack?: () => void): Promise<void> {
    const changeShowcaseImageObservable = this.httpClientService.put({
      controller: "products",
      action: "ChangeShowcaseImage"
    }, {
      imageId: imageId,
      productId: productId
    });
    await lastValueFrom(changeShowcaseImageObservable);
    successCallBack();
  }
}
