import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ForecastingTaskFieldValue } from '../../models/forecasting-task-field-value';

@Component({
  selector: 'app-add-dorecasting-task-data-dialog',
  templateUrl: './add-dorecasting-task-data-dialog.component.html',
  styleUrls: ['./add-dorecasting-task-data-dialog.component.css']
})
export class AddDorecastingTaskDataDialogComponent {

  obj: any = {};

  constructor(public dialogRef: MatDialogRef<AddDorecastingTaskDataDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: string) {}

    onCloseClick(): void {
      this.dialogRef.close();
    }

    onSubmitClick(): void {
      const result: ForecastingTaskFieldValue[] = [];

      // tslint:disable-next-line: forin
      for (const property in this.obj) {
        result.push({
          // tslint:disable-next-line: radix
          fieldId: parseInt(property),
          value: this.obj[property]
        });
      }

      this.dialogRef.close(result);
    }
}
