import { ComponentFactoryResolver, Injectable, ViewContainerRef } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class DynamicLoadComponentService {

  constructor(private componentFactoryResolver: ComponentFactoryResolver) { }

  async loadComponent(componentName: ComponentName, viewContainerRef: ViewContainerRef) {
    let component: any = null;

    switch (componentName) {
      case ComponentName.BasketsComponent:
        component = (await import("../../ui/components/baskets/baskets.component")).BasketsComponent;

        break;
    }

    viewContainerRef.clear();

    return viewContainerRef.createComponent(this.componentFactoryResolver.resolveComponentFactory(component));
  }
}

export enum ComponentName {
  BasketsComponent

}
