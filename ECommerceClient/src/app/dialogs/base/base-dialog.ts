import { MatDialogRef } from "@angular/material/dialog";

export class BaseDialog<DialogComponent> {
    constructor(
        public dialogRef: MatDialogRef<DialogComponent>
    ) { }

    close(closeCallback?: () => void): void {
        this.dialogRef.close();

        if (closeCallback != null)
            closeCallback();
    }
}
