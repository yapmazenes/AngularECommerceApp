import { Injectable, ViewContainerRef } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class DynamicLoadComponentService {

  constructor() { }

  async loadComponent(componentName: ComponentName, viewContainerRef: ViewContainerRef) {
    let component: any = null;

    switch (componentName) {
      case ComponentName.BasketsComponent:
        component = (await import("../../ui/components/baskets/baskets.component")).BasketsComponent;

        break;
    }

    viewContainerRef.clear();

    return viewContainerRef.createComponent(component);
  }
}

export enum ComponentName {
  BasketsComponent

}
