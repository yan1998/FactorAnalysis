import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ForecastingTaskFieldValue } from '../../models/forecasting-task-field-value';
import { ForecastingTaskFieldDeclaration } from '../../models/forecasting-task-field-declaration';

@Component({
  selector: 'app-predict-value-dialog',
  templateUrl: './predict-value-dialog.component.html',
  styleUrls: ['./predict-value-dialog.component.css']
})
export class PredictValueDialogComponent implements OnInit {

  obj: any = {};

  constructor(public dialogRef: MatDialogRef<PredictValueDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: ForecastingTaskFieldDeclaration[]) { }

  ngOnInit() {
  }

  onCloseClick(): void {
    this.dialogRef.close();
  }

  onSubmitClick(): void {
    const result: ForecastingTaskFieldValue[] = [];
    for (let i in this.data) {
      if (this.data[i]) {
        result.push({
          fieldId: this.data[i].id,
          value: this.obj[this.data[i].id].toString()
        });
      }
    }
    this.dialogRef.close(result);
  }

  isSubmitBtnDisabled(): boolean {
    for (let i in this.data) {
      if (!this.obj[this.data[i].id]) {
        return true;
      }
    }
    return false;
  }
}
