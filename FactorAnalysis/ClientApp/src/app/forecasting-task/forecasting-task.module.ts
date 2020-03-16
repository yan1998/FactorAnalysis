import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ForecastingTaskListComponent } from './forecastiong-task-list/forecasting-task-list.component';
import { ForecastingTaskComponent } from './forecasting-task.component';
import { ForecastingTaskRoutingModule } from './forecasting-task-routing.module';
import { ForecastingTaskService } from './services/forecasting-task.service';



@NgModule({
  declarations: [
    ForecastingTaskComponent,
    ForecastingTaskListComponent
  ],
  imports: [
    CommonModule,
    ForecastingTaskRoutingModule
  ],
  providers: [
    ForecastingTaskService
  ]
})
export class ForecastingTaskModule { }
