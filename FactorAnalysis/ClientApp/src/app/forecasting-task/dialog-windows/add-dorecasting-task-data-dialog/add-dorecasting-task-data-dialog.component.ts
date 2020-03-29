import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ForecastingTaskFactorValue } from '../../models/paged-forecasting-task';

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
      const result: ForecastingTaskFactorValue[] = [];

      // tslint:disable-next-line: forin
      for (const property in this.obj) {
        result.push({
          // tslint:disable-next-line: radix
          factorId: parseInt(property),
          // tslint:disable-next-line: radix
          value: parseInt(this.obj[property])
        });
      }

      this.dialogRef.close(result);
    }
}
