import { Component, OnInit } from '@angular/core';
import { CreateForecastingTaskEntityRequest } from '../models/requests/create-forecasting-task-entity-request';
import { ForecastingTaskService } from '../services/forecasting-task.service';
import { Router } from '@angular/router';
import { FieldType } from '../models/field-type.enum';
import { ForecastingTaskFieldDeclaration } from '../models/forecasting-task-field-declaration';

@Component({
  selector: 'app-forecasting-task-creation',
  templateUrl: './forecasting-task-creation.component.html',
  styleUrls: ['./forecasting-task-creation.component.css']
})
export class ForecastingTaskCreationComponent implements OnInit {

  taskName: string;
  taskFields: ForecastingTaskFieldDeclaration[];
  isPredicatedValueChecked = false;
  FieldType = FieldType;

  constructor(private _forecastingTaskService: ForecastingTaskService,
    private _router: Router) { }

  ngOnInit() {
    this.taskFields = [
      {
        id: 0,
        name: 'Фактор 1',
        description: 'Описание к фактору 1',
        type: FieldType.InformationField
      },
      {
        id: 0,
        name: 'Фактор 2',
        description: 'Описание к фактору 2',
        type: FieldType.Factor
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

    const request: CreateForecastingTaskEntityRequest = {
      taskEntityName: this.taskName,
      taskFieldsDeclaration: this.taskFields
    };

    this._forecastingTaskService.createForecatingTaskEntity(request).subscribe(x => {
      this._router.navigate(['/forecasting-task/list']);
    }, error => {
      console.log(error);
    });
  }
}
