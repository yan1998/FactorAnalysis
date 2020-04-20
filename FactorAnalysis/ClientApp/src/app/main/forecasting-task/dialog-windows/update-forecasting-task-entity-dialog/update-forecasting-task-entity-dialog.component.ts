import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { UpdateForecastingTaskEntityRequest } from '../../models/requests/update-forecasting-task-entity-request';
import { Validators, FormControl, FormGroup } from '@angular/forms';

@Component({
  selector: 'app-update-forecasting-task-entity-dialog',
  templateUrl: './update-forecasting-task-entity-dialog.component.html',
  styleUrls: ['./update-forecasting-task-entity-dialog.component.css']
})
export class UpdateForecastingTaskEntityDialogComponent implements OnInit {

  updateForm = new FormGroup({
    newName: new FormControl('', [Validators.required]),
    newDescription: new FormControl('')
  });

  constructor(public dialogRef: MatDialogRef<UpdateForecastingTaskEntityDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any) { }

  ngOnInit() {
    this.updateForm.get('newName').setValue(this.data.oldTaskName);
    this.updateForm.get('newDescription').setValue(this.data.oldTasDescription);
  }

  onCloseClick(): void {
    this.dialogRef.close();
  }

  onSubmitClick(): void {
    const result: UpdateForecastingTaskEntityRequest = {
      oldTaskName: this.data.oldTaskName,
      newTaskName: this.updateForm.get('newName').value,
      description: this.updateForm.get('newDescription').value
    };

    this.dialogRef.close(result);
  }
}
