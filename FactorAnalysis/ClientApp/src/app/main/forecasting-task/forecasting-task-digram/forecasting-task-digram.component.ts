import { Component, OnInit } from '@angular/core';
import { ForecastingTaskService } from '../services/forecasting-task.service';
import { GuiNotificatorService } from '../services/gui-notificator.service';
import { ActivatedRoute } from '@angular/router';
import { ForecastingTaskFieldDeclaration } from '../models/forecasting-task-field-declaration';
import { ForecastingTaskFieldValues } from '../models/responses/paged-forecasting-task-response';
import { FieldType } from '../models/field-type.enum';
import { element } from 'protractor';
import { parse } from 'url';
declare const CanvasJS: any;

@Component({
  selector: 'app-forecasting-task-digram',
  templateUrl: './forecasting-task-digram.component.html',
  styleUrls: ['./forecasting-task-digram.component.css']
})
export class ForecastingTaskDigramComponent implements OnInit {

  taskName: string;
  taskDeclaration: ForecastingTaskFieldDeclaration[];
  data: any;
  axisXField: string;
  axisYField: string;
  isDataLoading: boolean;

  constructor(private _forecastingTaskService: ForecastingTaskService,
    private route: ActivatedRoute,
    private _toastr: GuiNotificatorService) { }

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.taskName = params['name'];
    });

    this.isDataLoading = true;
    this._forecastingTaskService.getForecastingTaskDeclaration(this.taskName).subscribe(resultDeclaration => {
      this.taskDeclaration = resultDeclaration.fieldsDeclaration;
      this._forecastingTaskService.getAllForecastingTaskValues(this.taskName).subscribe(result => {
        this.createTaskObjectArray(result.fieldsValues);
        this.isDataLoading = false;
      }, error => {
        this._toastr.showError(error.error);
        this.isDataLoading = false;
      });
    }, error => {
      this._toastr.showError(error.error);
      this.isDataLoading = false;
    });
  }

  buildDiagram() {
    const dataPoints = [];
    this.data.forEach(element => {
      dataPoints.push({
        x: this.checkStringAndParse(element[this.axisXField]),
        y: parseFloat(element[this.axisYField])
      });
    });


    const chart = new CanvasJS.Chart('chartContainer', {
      zoomEnabled: true,
      animationEnabled: true,
      exportEnabled: true,
      title: {
        text: `Диаграмма ${this.taskName}`
      },
      data: [
      {
        type: 'line',
        dataPoints: dataPoints
      }]
    });

    chart.render();
  }

  buildBtnDisabled(): boolean {
    return !this.axisYField || !this.axisXField; 
  }

  getTaskFieldsExceptInformation(): ForecastingTaskFieldDeclaration[] {
    return this.taskDeclaration.filter(x => x.type !== FieldType.InformationField);
  }

  private checkStringAndParse(str: string): any {
    const floatRegExp = /^\d*.\d*$/;
    if (str.match(floatRegExp)) {
      return parseFloat(str);
    } else {
      return new Date(str);
    }
  }

  private createTaskObjectArray(fieldsValues: ForecastingTaskFieldValues[]) {
    const displayedFields = [];
    this.data = [];

    this.taskDeclaration.forEach(element => {
      displayedFields.push(element.name);
    });

    fieldsValues.forEach(element => {
      const obj = {};
      element.fieldsValue.forEach(field => {
        obj[displayedFields[field.fieldId]] = field.value;
      });
      this.data.push(obj);
    });
  }
}
