import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ForecastingTaskComponent } from './forecasting-task.component';
import { ForecastingTaskListComponent } from './forecastiong-task-list/forecasting-task-list.component';

const routes: Routes = [
    { path: '',  component: ForecastingTaskComponent , children: [
        { path: 'list', component: ForecastingTaskListComponent }
      ]
    }
];

@NgModule({
  imports: [ RouterModule.forChild(routes) ],
  exports: [ RouterModule ]
})
export class ForecastingTaskRoutingModule {}
