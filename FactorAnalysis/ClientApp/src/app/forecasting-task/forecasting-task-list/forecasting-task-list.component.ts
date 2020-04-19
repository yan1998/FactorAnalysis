import { Component, OnInit } from '@angular/core';
import { ForecastingTaskService } from '../services/forecasting-task.service';
import { Router } from '@angular/router';
import { ConfirmationDialogComponent } from 'src/app/dialog-windows/confirmation-dialog/confirmation-dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { GetForecastingTaskEntitiesResponse } from '../models/responses/get-forecasting-task-entities-response';

@Component({
  selector: 'app-forecasting-task-list',
  templateUrl: './forecasting-task-list.component.html',
  styleUrls: ['./forecasting-task-list.component.css']
})
export class ForecastingTaskListComponent implements OnInit {

  taskEntities: GetForecastingTaskEntitiesResponse[];

  constructor(private _forecastingTaskService: ForecastingTaskService,
    private _router: Router,
    private dialog: MatDialog) { }

  ngOnInit() {
    this.retrieveData();
  }

  goToCreation(): void {
    this._router.navigate(['/forecasting-task/task-creation']);
  }

  deleteTaskEntity(name: string): void {

    const dialogRef = this.dialog.open(ConfirmationDialogComponent, {
      width: '300px',
      data: 'Вы действительно хотите удалить задачу?'
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this._forecastingTaskService.deleteForecastingTaskEntity(name).subscribe(() => {
          this.retrieveData();
        }, error => console.log(error));
      }
    });
  }

  private retrieveData() {
    this._forecastingTaskService.getForecastingTaskEntities()
      .subscribe(data => {
        this.taskEntities = data;
      }, error => {
        console.log(error);
      });
  }
}
