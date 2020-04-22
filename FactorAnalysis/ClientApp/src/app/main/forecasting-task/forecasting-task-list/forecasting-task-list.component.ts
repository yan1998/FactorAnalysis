import { Component, OnInit } from '@angular/core';
import { ForecastingTaskService } from '../services/forecasting-task.service';
import { Router } from '@angular/router';
import { ConfirmationDialogComponent } from 'src/app/main/dialog-windows/confirmation-dialog/confirmation-dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { GetForecastingTaskEntitiesResponse } from '../models/responses/get-forecasting-task-entities-response';
import { UpdateForecastingTaskEntityDialogComponent } from '../dialog-windows/update-forecasting-task-entity-dialog/update-forecasting-task-entity-dialog.component';

@Component({
  selector: 'app-forecasting-task-list',
  templateUrl: './forecasting-task-list.component.html',
  styleUrls: ['./forecasting-task-list.component.css']
})
export class ForecastingTaskListComponent implements OnInit {

  isDataLoading: boolean;
  searchField: string;
  taskEntities: GetForecastingTaskEntitiesResponse[];
  filteredTaskEntities: GetForecastingTaskEntitiesResponse[];

  constructor(private _forecastingTaskService: ForecastingTaskService,
    private _router: Router,
    private dialog: MatDialog) { }

  ngOnInit() {
    this.retrieveData();
  }

  goToCreation(): void {
    this._router.navigate(['/forecasting-task/task-creation']);
  }

  search(): void {
    this.filteredTaskEntities = this.taskEntities.filter(x => x.name.toLowerCase().includes(this.searchField.toLowerCase()));
  }

  editTaskEntity(taskEntity: GetForecastingTaskEntitiesResponse): void {
    const dialogRef = this.dialog.open(UpdateForecastingTaskEntityDialogComponent, {
      width: '300px',
      data: {
        oldTaskName: taskEntity.name,
        oldTasDescription: taskEntity.description
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.isDataLoading = true;
        this._forecastingTaskService.updateForecastingTaskEntity(result).subscribe(() => {
          this.retrieveData();
        }, error => {
          this.isDataLoading = false;
          console.log(error);
        });
      }
    });
  }

  deleteTaskEntity(name: string): void {
    const dialogRef = this.dialog.open(ConfirmationDialogComponent, {
      width: '300px',
      data: 'Вы действительно хотите удалить задачу?'
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.isDataLoading = true;
        this._forecastingTaskService.deleteForecastingTaskEntity(name).subscribe(() => {
          this.retrieveData();
        }, error => {
          this.isDataLoading = false;
          console.log(error);
        });
      }
    });
  }

  private retrieveData() {
    this.isDataLoading = true;
    this._forecastingTaskService.getForecastingTaskEntities()
      .subscribe(data => {
        this.isDataLoading = false;
        this.filteredTaskEntities = data;
        this.taskEntities = data;
      }, error => {
        this.isDataLoading = false;
        console.log(error);
      });
  }
}
