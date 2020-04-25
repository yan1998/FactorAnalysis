import { Component, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { LearningAlgorithm } from '../../models/learning-algorithm.enum';

@Component({
  selector: 'app-create-task-entity-prediction-model-dialog',
  templateUrl: './create-task-entity-prediction-model-dialog.component.html',
  styleUrls: ['./create-task-entity-prediction-model-dialog.component.css']
})
export class CreateTaskEntityPredictionModelDialogComponent {

  LearningAlgorithm = LearningAlgorithm;
  selectedAlgorithm: LearningAlgorithm;

  constructor(public dialogRef: MatDialogRef<CreateTaskEntityPredictionModelDialogComponent>) { }

  onCloseClick(): void {
    this.dialogRef.close();
  }

  onSubmitClick(): void {
    this.dialogRef.close(this.selectedAlgorithm);
  }

  learningAlgorithmKeys(): string[] {
    const keys = Object.keys(LearningAlgorithm);
    return keys.slice(keys.length / 2);
  }
}
