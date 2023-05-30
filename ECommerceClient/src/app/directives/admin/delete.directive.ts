import { Directive, ElementRef, EventEmitter, HostListener, Input, Output, Renderer2 } from '@angular/core';
import { ProductService } from 'src/app/services/common/product.service';

declare var $: any;

@Directive({
  selector: '[appDelete]'
})
export class DeleteDirective {

  constructor(
    private element: ElementRef,
    private _renderer: Renderer2,
    private productService: ProductService) {

    const image = _renderer.createElement("img");
    image.setAttribute("src", "../../../../../assets/admin-icons/delete.png");
    image.setAttribute("style", "cursor:pointer");
    _renderer.appendChild(element.nativeElement, image);
  }

  @Input() id: string;
  @Output() callback: EventEmitter<any> = new EventEmitter();

  @HostListener("click")
  async onClick() {
    const td: HTMLTableCellElement = this.element.nativeElement;
    await this.productService.delete(this.id)
    $(td.parentElement).fadeOut(1000, () => {
      this.callback.emit();
    });
  }

}
