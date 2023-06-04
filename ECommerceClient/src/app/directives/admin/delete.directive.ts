import { HttpErrorResponse } from '@angular/common/http';
import { Directive, ElementRef, EventEmitter, HostListener, Input, Output, Renderer2 } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { NgxSpinnerService } from 'ngx-spinner';
import { SpinnerType } from 'src/app/base/base.component';
import { DeleteDialogComponent, DeleteState } from 'src/app/dialogs/delete-dialog/delete-dialog.component';
import { AlertifyService, MessageType, Position } from 'src/app/services/admin/alertify.service';
import { DialogService } from 'src/app/services/common/dialog.service';
import { HttpClientService } from 'src/app/services/common/http-client.service';

declare var $: any;

@Directive({
  selector: '[appDelete]'
})
export class DeleteDirective {

  constructor(
    private element: ElementRef,
    private _renderer: Renderer2,
    private httpClientService: HttpClientService,
    public dialog: MatDialog,
    private spinner: NgxSpinnerService,
    private alertifyService: AlertifyService,
    private dialogService: DialogService
  ) {

    const image = _renderer.createElement("img");
    image.setAttribute("src", "../../../../../assets/admin-icons/delete.png");
    image.setAttribute("style", "cursor:pointer");
    _renderer.appendChild(element.nativeElement, image);
  }

  @Input() id: string;
  @Input() controller: string;
  @Output() callback: EventEmitter<any> = new EventEmitter();

  @HostListener("click")
  async onClick() {
    this.dialogService.openDialog({
      componentType: DeleteDialogComponent,
      data: DeleteState.Yes,
      afterClosed: async () => {
        const td: HTMLTableCellElement = this.element.nativeElement;
        this.httpClientService.delete({ action: this.controller }, this.id).subscribe(data => {
          $(td.parentElement).animate({
            opacity: 0,
            left: "+=50",
            height: "toogle"
          }, 700, () => {
            this.callback.emit();
            this.alertifyService.message("Kayıt başarıyla silinmiştir",
              { dismissOthers: true, messageType: MessageType.Success, position: Position.TopRight });
          });
        }, (errorResponse: HttpErrorResponse) => {
          this.spinner.hide(SpinnerType.BallAtom)
          this.alertifyService.message("Kayıt silinme sırasında bir hata oluştu",
            { dismissOthers: true, messageType: MessageType.Error, position: Position.TopRight });
        });

      }
    });
  }
}