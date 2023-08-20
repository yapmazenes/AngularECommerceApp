import { Injectable } from '@angular/core';
import { HttpClientService } from './http-client.service';
import { Observable, lastValueFrom } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class QrCodeService {

  constructor(private httpClientService: HttpClientService) { }

  async generateQRCodeByProduct(productId: string) {
    const observable: Observable<Blob> = this.httpClientService.get({
      controller: "products",
      action: "qrcode",
      responseType: "blob"
    }, productId);

    return await lastValueFrom(observable);
  }
}
