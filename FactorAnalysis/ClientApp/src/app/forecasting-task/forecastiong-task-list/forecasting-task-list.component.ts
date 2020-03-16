import { Component, OnInit } from '@angular/core';
import { ForecastingTaskService } from '../services/forecasting-task.service';

@Component({
  selector: 'app-forecasting-task-list',
  templateUrl: './forecasting-task-list.component.html',
  styleUrls: ['./forecasting-task-list.component.css']
})
export class ForecastingTaskListComponent implements OnInit {

  private taskEntitiesNames: string[];

  constructor(private _forecastingTaskService: ForecastingTaskService) { }

  ngOnInit() {
    this._forecastingTaskService.getForecastingTaskEntitiesName()
      .subscribe(data => {
        this.taskEntitiesNames = data;
      }, error => {
        console.log(error);
      });
  }

}
