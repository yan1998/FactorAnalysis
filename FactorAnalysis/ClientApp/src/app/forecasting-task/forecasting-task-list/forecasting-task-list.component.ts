import { Component, OnInit } from '@angular/core';
import { ForecastingTaskService } from '../services/forecasting-task.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-forecasting-task-list',
  templateUrl: './forecasting-task-list.component.html',
  styleUrls: ['./forecasting-task-list.component.css']
})
export class ForecastingTaskListComponent implements OnInit {

  taskEntitiesNames: string[];

  constructor(private _forecastingTaskService: ForecastingTaskService,
    private _router: Router) { }

  ngOnInit() {
    this._forecastingTaskService.getForecastingTaskEntitiesName()
      .subscribe(data => {
        this.taskEntitiesNames = data;
      }, error => {
        console.log(error);
      });
  }

  goToCreation(): void {
    this._router.navigate(['/forecasting-task/task-creation']);
  }
}
