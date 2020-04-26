import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { ForecastingTaskFieldValue } from '../../models/forecasting-task-field-value';
import { FieldType } from '../../models/field-type.enum';
import { ForecastingTaskFieldDeclaration } from '../../models/forecasting-task-field-declaration';

@Component({
  selector: 'app-add-forecasting-task-data-dialog',
  templateUrl: './add-forecasting-task-data-dialog.component.html',
  styleUrls: ['./add-forecasting-task-data-dialog.component.css']
})
export class AddForecastingTaskDataDialogComponent {

  obj: any = {};
  FieldType = FieldType;

  constructor(public dialogRef: MatDialogRef<AddForecastingTaskDataDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: ForecastingTaskFieldDeclaration[]) {}

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
      if (!this.obj[this.data[i].id] || this.obj[this.data[i].id].trim() === '') {
        return true;
      }
    }
    return false;
  }
}
