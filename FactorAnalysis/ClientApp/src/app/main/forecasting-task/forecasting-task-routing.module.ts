import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ForecastingTaskComponent } from './forecasting-task.component';
import { ForecastingTaskListComponent } from './forecasting-task-list/forecasting-task-list.component';
import { ForecastingTaskCreationComponent } from './forecasting-task-creation/forecasting-task-creation.component';
import { DisplayTaskComponent } from './display-task/display-task.component';
import { ForecastingTaskDigramComponent } from './forecasting-task-digram/forecasting-task-digram.component';
import { AnalyzePredictionAlgorithmsComponent } from './analyze-prediction-algorithms/analyze-prediction-algorithms.component';

const routes: Routes = [
    { path: '',  component: ForecastingTaskComponent , children: [
        { path: 'list', component: ForecastingTaskListComponent },
        { path: 'task-creation', component: ForecastingTaskCreationComponent },
        { path: 'display-task/:name', component: DisplayTaskComponent },
        { path: 'task-diagram/:name', component: ForecastingTaskDigramComponent },
        { path: 'analyze-prediction-algorithms/:name', component: AnalyzePredictionAlgorithmsComponent }
      ]
    }
];

@NgModule({
  imports: [ RouterModule.forChild(routes) ],
  exports: [ RouterModule ]
})
export class ForecastingTaskRoutingModule {}
