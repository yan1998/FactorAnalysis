import { Component, OnInit } from '@angular/core';
import { CreateForecastingTaskEntityRequest } from '../models/requests/create-forecasting-task-entity-request';
import { ForecastingTaskService } from '../services/forecasting-task.service';
import { Router } from '@angular/router';
import { FieldType } from '../models/field-type.enum';
import { ForecastingTaskFieldDeclaration } from '../models/forecasting-task-field-declaration';
import { GuiNotificatorService } from '../services/gui-notificator.service';

@Component({
  selector: 'app-forecasting-task-creation',
  templateUrl: './forecasting-task-creation.component.html',
  styleUrls: ['./forecasting-task-creation.component.css']
})
export class ForecastingTaskCreationComponent implements OnInit {

  taskName: string;
  taskDescription: string;
  taskFields: ForecastingTaskFieldDeclaration[];
  isPredicatedValueChecked = false;
  FieldType = FieldType;
  isTaskCreating: boolean;

  constructor(private _forecastingTaskService: ForecastingTaskService,
    private _router: Router,
    private _toastr: GuiNotificatorService) { }

  ngOnInit() {
    this.taskFields = [
      {
        id: 0,
        name: 'Поле 1',
        description: 'Описание к полю 1',
        type: FieldType.PredictionField
      },
      {
        id: 0,
        name: 'Поле 2',
        description: 'Описание к полю 2',
        type: FieldType.Factor
      },
      {
        id: 0,
        name: 'Поле 3',
        description: 'Описание к полю 3',
        type: FieldType.InformationField
      }
    ];
  }

  public addField(): void {
    const newField: ForecastingTaskFieldDeclaration = {
      id: 0,
      description: '',
      name: '',
      type: FieldType.InformationField
    };

    this.taskFields.push(newField);
  }

 public removeField(factor: ForecastingTaskFieldDeclaration): void {
    const index = this.taskFields.indexOf(factor);
    if (index > -1) {
      this.taskFields.splice(index, 1);
    }
  }

  public createTask(): void {
    this.isTaskCreating = true;

    const request: CreateForecastingTaskEntityRequest = {
      name: this.taskName,
      description: this.taskDescription,
      fieldsDeclaration: this.taskFields
    };

    this._forecastingTaskService.createForecatingTaskEntity(request).subscribe(x => {
      this.isTaskCreating = false;
      this._toastr.showSuccess('Задача прогнозирования была успешно создана!');
      this._router.navigate(['/forecasting-task/list']);
    }, error => {
      this.isTaskCreating = false;
      this._toastr.showError(error.error);
    });
  }

  doesPredictionFieldSelected(): boolean {
    return this.taskFields.some(x => x.type === FieldType.PredictionField);
  }

  isValid(): boolean {
    return this.doesPredictionFieldSelected()
      && this.taskFields.some(x => x.type === FieldType.Factor)
      && !this.taskFields.some(x => x.name === '')
      && (this.taskName && this.taskName !== '');
  }
}
