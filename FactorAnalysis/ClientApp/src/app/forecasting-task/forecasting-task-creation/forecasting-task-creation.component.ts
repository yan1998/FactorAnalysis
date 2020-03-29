import { Component, OnInit } from '@angular/core';
import { ForecastingTaskFactorDeclarationCreationRequest, CreateForecastingTaskEntityRequest } from '../models/create-forecasting-task-entity-request';
import { ForecastingTaskService } from '../services/forecasting-task.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-forecasting-task-creation',
  templateUrl: './forecasting-task-creation.component.html',
  styleUrls: ['./forecasting-task-creation.component.css']
})
export class ForecastingTaskCreationComponent implements OnInit {

  taskName: string;
  taskFactors: ForecastingTaskFactorDeclarationCreationRequest[];
  isPredicatedValueChecked = false;

  constructor(private _forecastingTaskService: ForecastingTaskService,
    private _router: Router) { }

  ngOnInit() {
    this.taskFactors = [
      {
        name: 'Фактор 1',
        description: 'Описание к фактору 1',
        isPredicatedValue: false
      },
      {
        name: 'Фактор 2',
        description: 'Описание к фактору 2',
        isPredicatedValue: false
      }
    ];
  }

  public addFactor(): void {
    const newFactor: ForecastingTaskFactorDeclarationCreationRequest = {
      description: '',
      name: '',
      isPredicatedValue: false
    };

    this.taskFactors.push(newFactor);
  }

  public removeFactor(factor: ForecastingTaskFactorDeclarationCreationRequest): void {
    const index = this.taskFactors.indexOf(factor);
    if (index > -1) {
      this.taskFactors.splice(index, 1);
    }

    if (factor.isPredicatedValue) {
      this.isPredicatedValueChecked = false;
    }
  }

  public createTask(): void {

    const request: CreateForecastingTaskEntityRequest = {
      taskEntityName: this.taskName,
      taskFactorsDeclaration: this.taskFactors
    };

    this._forecastingTaskService.createForecatingTaskEntity(request).subscribe(x => {
      this._router.navigate(['/forecasting-task/list']);
    }, error => {
      console.log(error);
    });
  }
}
