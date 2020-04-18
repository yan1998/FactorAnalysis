import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ForecastingTaskFieldValue } from '../../models/forecasting-task-field-value';

@Component({
  selector: 'app-predict-value-dialog',
  templateUrl: './predict-value-dialog.component.html',
  styleUrls: ['./predict-value-dialog.component.css']
})
export class PredictValueDialogComponent implements OnInit {

  obj: any = {};

  constructor(public dialogRef: MatDialogRef<PredictValueDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: string) { }

  ngOnInit() {
  }

  onCloseClick(): void {
    this.dialogRef.close();
  }

  onSubmitClick(): void {
  {
    const result: ForecastingTaskFieldValue[] = [];

      // tslint:disable-next-line: forin
      for (const property in this.obj) {
        result.push({
          // tslint:disable-next-line: radix
          fieldId: parseInt(property),
          value: this.obj[property].toString()
        });
      }

      this.dialogRef.close(result);
    }
  }
}
