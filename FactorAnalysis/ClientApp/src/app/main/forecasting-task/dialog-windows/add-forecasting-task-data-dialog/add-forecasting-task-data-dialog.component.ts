import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ForecastingTaskFieldValue } from '../../models/forecasting-task-field-value';

@Component({
  selector: 'app-add-forecasting-task-data-dialog',
  templateUrl: './add-forecasting-task-data-dialog.component.html',
  styleUrls: ['./add-forecasting-task-data-dialog.component.css']
})
export class AddForecastingTaskDataDialogComponent {

  obj: any = {};

  constructor(public dialogRef: MatDialogRef<AddForecastingTaskDataDialogComponent>,
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
