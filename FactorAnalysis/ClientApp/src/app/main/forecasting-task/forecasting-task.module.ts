import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ForecastingTaskListComponent } from './forecasting-task-list/forecasting-task-list.component';
import { ForecastingTaskComponent } from './forecasting-task.component';
import { ForecastingTaskRoutingModule } from './forecasting-task-routing.module';
import { ForecastingTaskService } from './services/forecasting-task.service';
import { ForecastingTaskCreationComponent } from './forecasting-task-creation/forecasting-task-creation.component';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { DisplayTaskComponent } from './display-task/display-task.component';
import { MatButtonModule } from '@angular/material/button';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatDialogModule } from '@angular/material/dialog';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { AddForecastingTaskDataDialogComponent } from './dialog-windows/add-forecasting-task-data-dialog/add-forecasting-task-data-dialog.component';
import { FileDownloaderService } from './services/file-downloader.service';
import { MatSelectModule } from '@angular/material/select';
import { PredictValueDialogComponent } from './dialog-windows/predict-value-dialog/predict-value-dialog.component';
import { MatIconModule } from '@angular/material/icon';
import { MatTooltipModule } from '@angular/material/tooltip';
import { UpdateForecastingTaskEntityDialogComponent } from './dialog-windows/update-forecasting-task-entity-dialog/update-forecasting-task-entity-dialog.component';
import { GuiNotificatorService } from './services/gui-notificator.service';
import { ToastrModule } from 'ngx-toastr';
import { CreateTaskEntityPredictionModelDialogComponent } from './dialog-windows/create-task-entity-prediction-model-dialog/create-task-entity-prediction-model-dialog.component';
import { MatExpansionModule } from '@angular/material/expansion';
import { ForecastingTaskDigramComponent } from './forecasting-task-digram/forecasting-task-digram.component';

@NgModule({
  declarations: [
    ForecastingTaskComponent,
    ForecastingTaskListComponent,
    ForecastingTaskCreationComponent,
    DisplayTaskComponent,
    AddForecastingTaskDataDialogComponent,
    PredictValueDialogComponent,
    UpdateForecastingTaskEntityDialogComponent,
    CreateTaskEntityPredictionModelDialogComponent,
    ForecastingTaskDigramComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    ForecastingTaskRoutingModule,
    MatCheckboxModule,
    MatButtonModule,
    MatTableModule,
    MatPaginatorModule,
    MatProgressSpinnerModule,
    MatDialogModule,
    MatSelectModule,
    MatInputModule,
    MatFormFieldModule,
    MatIconModule,
    MatTooltipModule,
    MatExpansionModule,
    ToastrModule.forRoot({
      timeOut: 5000,
      positionClass: 'toast-bottom-right',
      maxOpened: 3
    })
  ],
  providers: [
    ForecastingTaskService,
    FileDownloaderService,
    GuiNotificatorService
  ],
  entryComponents: [
    AddForecastingTaskDataDialogComponent,
    PredictValueDialogComponent,
    UpdateForecastingTaskEntityDialogComponent,
    CreateTaskEntityPredictionModelDialogComponent
  ]
})
export class ForecastingTaskModule { }
