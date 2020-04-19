import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-predicate-value-dialog',
  templateUrl: './predicate-value-dialog.component.html',
  styleUrls: ['./predicate-value-dialog.component.css']
})
export class PredicateValueDialogComponent implements OnInit {

  constructor(public dialogRef: MatDialogRef<PredicateValueDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: string) { }

  ngOnInit() {
  }

  onNoClick(): void {
    this.dialogRef.close();
  }

  onYesClick(): void {
    this.dialogRef.close();
  }
}
