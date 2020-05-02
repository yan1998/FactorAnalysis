import { Component, OnInit } from '@angular/core';
import { ForecastingTaskService } from '../services/forecasting-task.service';
import { ActivatedRoute } from '@angular/router';
import { GuiNotificatorService } from '../services/gui-notificator.service';
import { LearningAlgorithm } from '../models/learning-algorithm.enum';
import { AnalyzePredictionAlgorithmsRequest } from '../models/requests/analyze-prediction-algorithms-request';
import { MatCheckboxChange } from '@angular/material/checkbox';
import { AnalyzePredictionAlgorithmsReport } from '../models/analyze-prediction-algorithms-report';
import { ForecastingTaskFieldDeclaration } from '../models/forecasting-task-field-declaration';
import { AlgorithmPredictionResult } from '../models/algorithm-prediction-result';
import { FieldType } from '../models/field-type.enum';

@Component({
  selector: 'app-analyze-prediction-algorithms',
  templateUrl: './analyze-prediction-algorithms.component.html',
  styleUrls: ['./analyze-prediction-algorithms.component.css']
})
export class AnalyzePredictionAlgorithmsComponent implements OnInit {

  taskName: string;
  isPanelExpended: boolean;
  isTestngInProgress: boolean;
  isLoadingDeclaration: boolean;
  taskDeclaration: ForecastingTaskFieldDeclaration[];
  displayedColumns: string[] = [];
  selectedAlgorithms: string[] = [];
  reports: AnalyzePredictionAlgorithmsReport[];
  LearningAlgorithm = LearningAlgorithm;

  constructor(private _forecastingTaskService: ForecastingTaskService,
    private route: ActivatedRoute,
    private _toastr: GuiNotificatorService) { }

  ngOnInit() {
    this.isPanelExpended = true;
    this.route.params.subscribe(params => {
      this.taskName = params['name'];
    });

    this._forecastingTaskService.getForecastingTaskDeclaration(this.taskName).subscribe((response) => {
      this.taskDeclaration = response.fieldsDeclaration;
      this.taskDeclaration.sort(this.compare).forEach(element => {
        if (element.type !== FieldType.InformationField) {
          this.displayedColumns.push(element.name);
        }
      });
      this.displayedColumns.push('Results');
      this.isLoadingDeclaration = false;
    }, error => {
      this._toastr.showError(error.error);
      this.isLoadingDeclaration = false;
    });
  }

  startTesting() {
    this.isTestngInProgress = true;
    this.reports = null;
    const request: AnalyzePredictionAlgorithmsRequest = {
      taskEntityName: this.taskName,
      algorithms: this.selectedAlgorithms
    };
    this._forecastingTaskService.analyzePredictionAlgorithms(request).subscribe(response => {
      this.isTestngInProgress = false;
      this.isPanelExpended = false;
      this.reports = response.reports;
    }, error => {
      this.isTestngInProgress = false;
      this._toastr.showError(error.error);
    });
  }

  selectedAlgorithmsChanged(event: MatCheckboxChange, algorithm: LearningAlgorithm) {
    if (event.checked) {
      this.selectedAlgorithms.push(algorithm.toString());
    } else {
      const index = this.selectedAlgorithms.indexOf(algorithm.toString());
      if (index > -1) {
        this.selectedAlgorithms.splice(index, 1);
      }
    }
  }

  learningAlgorithmKeys(): string[] {
    const keys = Object.keys(LearningAlgorithm);
    return keys.slice(keys.length / 2);
  }

  formatElapsedTime(time: any): string {
    const minutes = time.minutes < 10 ? `0${time.minutes}` : `${time.minutes}`;
    const seconds = time.seconds < 10 ? `0${time.seconds}` : `${time.seconds}`;
    return `${minutes}:${seconds}:${time.milliseconds}`;
  }

  getDataSource(results: AlgorithmPredictionResult[]): any {
    const dataSource = [];

    results.forEach(result => {
      const obj = {};
      result.factorValues.forEach(field => {
        const name = this.taskDeclaration.filter(x => x.id === field.fieldId)[0].name;
        obj[name] = field.value;
      });
      obj['Results'] = result.result;
      dataSource.push(obj);
    });

    return dataSource;
  }

  private compare(a: ForecastingTaskFieldDeclaration, b: ForecastingTaskFieldDeclaration) {
    if (a.type < b.type) {
      return -1;
    }
    if (a.type > b.type) {
      return 1;
    }
    return 0;
  }
}
