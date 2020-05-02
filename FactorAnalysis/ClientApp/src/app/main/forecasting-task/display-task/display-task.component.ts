import { Component, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ForecastingTaskService } from '../services/forecasting-task.service';
import { PagedForecastingTaskResponse } from '../models/responses/paged-forecasting-task-response';
import { MatPaginator } from '@angular/material/paginator';
import { merge, of as observableOf } from 'rxjs';
import { startWith, switchMap, map, catchError } from 'rxjs/operators';
import { MatDialog } from '@angular/material/dialog';
import { ConfirmationDialogComponent } from 'src/app/main/dialog-windows/confirmation-dialog/confirmation-dialog.component';
import { AddForecastingTaskDataDialogComponent } from '../dialog-windows/add-forecasting-task-data-dialog/add-forecasting-task-data-dialog.component';
import { FileDownloaderService } from '../services/file-downloader.service';
import { PredictValueDialogComponent } from '../dialog-windows/predict-value-dialog/predict-value-dialog.component';
import { FieldType } from '../models/field-type.enum';
import { PredictValueRequest } from '../models/requests/predict-value-request';
import { ForecastingTaskFieldValueRequest } from '../models/requests/forecasting-task-field-value-request';
import { GuiNotificatorService } from '../services/gui-notificator.service';
import { CreateTaskEntityPredictionModelDialogComponent } from '../dialog-windows/create-task-entity-prediction-model-dialog/create-task-entity-prediction-model-dialog.component';
import { GetPagedForecastingTaskRequest } from '../models/requests/get-paged-forecasting-task-request';
import { ForecastingTaskFieldValue } from '../models/forecasting-task-field-value';
import { ForecastingTaskFieldDeclaration } from '../models/forecasting-task-field-declaration';

@Component({
  selector: 'app-display-task',
  templateUrl: './display-task.component.html',
  styleUrls: ['./display-task.component.css']
})
export class DisplayTaskComponent implements OnInit, AfterViewInit {

  @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
  name: string;
  taskRecords: PagedForecastingTaskResponse;
  taskDeclaration: ForecastingTaskFieldDeclaration[];
  resultsLength: number;
  displayedColumns: string[];
  isLoadingResults: boolean;
  isLoadingDeclaration: boolean;
  data: any;
  isCsvUploading: boolean;
  isCsvDownloading: boolean;
  isJsonDownloading: boolean;
  isXmlDownloading: boolean;
  isModelCreating: boolean;
  isDataAdding: boolean;
  isValuePredicating: boolean;
  searchFilters: ForecastingTaskFieldValue[];
  searchFiltersRequest: ForecastingTaskFieldValue[];

  constructor(private _forecastingTaskService: ForecastingTaskService,
    private _fileDownloaderService: FileDownloaderService,
    private route: ActivatedRoute,
    private dialog: MatDialog,
    private _toastr: GuiNotificatorService) { }

  ngOnInit() {
    this.isLoadingResults = true;
    this.isLoadingDeclaration = true;
    this.data = [];
    this.taskRecords = {
      fieldsValues: [],
      name: '',
      totalCount: 0
    };

    this.searchFilters = [
      {
        fieldId: null,
        value: null
      }
    ];

    this.displayedColumns = [];
    this.route.params.subscribe(params => {
      this.name = params['name'];
    });

    this._forecastingTaskService.getForecastingTaskDeclaration(this.name).subscribe((response) => {
      this.taskDeclaration = response.fieldsDeclaration;
      this.isLoadingDeclaration = false;
    }, error => {
      this._toastr.showError(error.error);
      this.isLoadingDeclaration = false;
    });
  }

  ngAfterViewInit() {
    merge(this.paginator.page).pipe(
      startWith({}),
      switchMap(() => {
        this.isLoadingResults = true;
        const request: GetPagedForecastingTaskRequest = {
          taskEntityName: this.name,
          pageNumber: this.paginator.pageIndex + 1,
          perPage: this.paginator.pageSize,
          forecastingTaskFieldValues: this.searchFiltersRequest
        };
        return this._forecastingTaskService.getPagedForecastingTask(request);
      }),
      map(data => {
        return data;
      }),
      catchError(() => {
        this.isLoadingResults = false;
        return observableOf([]);
      })
    ).subscribe(taskRecords => {
      this.taskRecords = taskRecords as PagedForecastingTaskResponse;
      this.resultsLength = this.taskRecords.totalCount;
      this.createArray();
      this.isLoadingResults = false;
    });
  }

  removeRecord(id: number) {
    const dialogRef = this.dialog.open(ConfirmationDialogComponent, {
      width: '300px',
      data: 'Вы действительно хотите удалить запись?'
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this._forecastingTaskService.deleteForecastingTaskEntityValues(this.name, id).subscribe(() => {
          this.paginator.pageIndex = 1;
          this.paginator.firstPage();
          this._toastr.showSuccess('Запись была успешно удалена!');
        }, error => {
          this._toastr.showError(error.error);
        });
      }
    });
  }

  addForecastingTaskData() {
    const factors = [];
    this.taskDeclaration.forEach(element => {
      factors.push({
        id: element.id,
        name: element.name,
        type: element.type
      });
    });

    const dialogRef = this.dialog.open(AddForecastingTaskDataDialogComponent, {
      width: '300px',
      data: factors
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.isDataAdding = true;
        const request: ForecastingTaskFieldValueRequest = {
          values: result
        };
        this._forecastingTaskService.addForecstingTaskFactorsValue(this.name, request).subscribe(() => {
          this.paginator.pageIndex = 1;
          this.paginator.firstPage();
          this.isDataAdding = false;
          this._toastr.showSuccess('Запись была успешно добавлена!');
        }, error => {
          this.isDataAdding = false;
          this._toastr.showError(error.error);
        });
      }
    });
  }

  exportCsv() {
    this.isCsvDownloading = true;
    this._forecastingTaskService.saveForecastingTaskEntityCsv(this.name).subscribe(file => {
      const blob = new Blob([file], { type: 'text/csv' });
      this._fileDownloaderService.downloadBlob(blob, `${this.name}.csv`);
      this.isCsvDownloading = false;
    }, (error) => {
      this.isCsvDownloading = false;
      this._toastr.showError(error.error);
    });
  }

  uploadCsvFile(files: File[]) {
    if (files.length === 0) {
      return;
    }

    this.isCsvUploading = true;
    const fileToUpload = <File>files[0];
    const formData = new FormData();
    formData.append('file', fileToUpload, fileToUpload.name);

    this._forecastingTaskService.uploadForecastingTaskValuesCsv(this.name, formData).subscribe(() => {
      this.paginator.pageIndex = 1;
      this.paginator.firstPage();
      this.isCsvUploading = false;
    }, error => {
      this.isCsvUploading = false;
      this._toastr.showError(error.error);
    });
  }

  exportJson() {
    this.isJsonDownloading = true;
    this._forecastingTaskService.saveForecastingTaskEntityJson(this.name).subscribe(file => {
      const blob = new Blob([file], { type: 'text/csv' });
      this._fileDownloaderService.downloadBlob(blob, `${this.name}.json`);
      this.isJsonDownloading = false;
    }, (error) => {
      this.isJsonDownloading = false;
      this._toastr.showError(error.error);
    });
  }

  exportXml() {
    this.isXmlDownloading = true;
    this._forecastingTaskService.saveForecastingTaskEntityXml(this.name).subscribe(file => {
      const blob = new Blob([file], { type: 'text/csv' });
      this._fileDownloaderService.downloadBlob(blob, `${this.name}.xml`);
      this.isXmlDownloading = false;
    }, (error) => {
      this.isXmlDownloading = false;
      this._toastr.showError(error.error);
    });
  }

  createPredictionModel() {
    const dialogRef = this.dialog.open(CreateTaskEntityPredictionModelDialogComponent, {
      width: '300px'
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.isModelCreating = true;
        this._forecastingTaskService.сreateTaskEntityPredictionModel(this.name, result).subscribe(() => {
          this._toastr.showSuccess('Обучение прошло успешно');
          this.isModelCreating = false;
        }, error => {
          this.isModelCreating = false;
          this._toastr.showError(error.error);
        });
      }
    });
  }

  predictValue() {
    const dialogRef = this.dialog.open(PredictValueDialogComponent, {
      width: '400px',
      data: this.taskDeclaration.filter(x => x.type === FieldType.Factor)
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.isValuePredicating = true;
        const request: PredictValueRequest = {
          values: result
        };
        this._forecastingTaskService.predictValue(this.name, request).subscribe((value) => {
          this.isValuePredicating = false;
          this._toastr.showInfo('Прогнозируемое значение = ' + value);
        }, error => {
          this.isValuePredicating = false;
          this._toastr.showError(error.error);
        });
      }
    });
  }

  addFilter() {
    this.searchFilters.push({
      fieldId: null,
      value: null
    });
  }

  removeFilter(filter) {
    const index = this.searchFilters.indexOf(filter);
    if (index > -1) {
      this.searchFilters.splice(index, 1);
    }
  }

  search() {
    this.searchFiltersRequest = [];
    this.searchFiltersRequest = this.searchFiltersRequest.concat(this.searchFilters);
    this.paginator.pageIndex = 1;
    this.paginator.firstPage();
  }

  resetFilters() {
    this.searchFilters = [{
      fieldId: null,
      value: null
    }];

    this.searchFiltersRequest = [];
    this.paginator.pageIndex = 1;
    this.paginator.firstPage();
  }

  isSearchDisabled(): boolean {
    return this.searchFilters.some(x => x.fieldId == null || x.value == null || x.value.trim() === '' );
  }

  private createArray() {
    this.displayedColumns = [];
    this.data = [];

    this.taskDeclaration.forEach(element => {
      this.displayedColumns.push(element.name);
    });
    this.displayedColumns.push('delete');

    this.taskRecords.fieldsValues.forEach(element => {
      const obj = {
        id: element.id
      };
      element.fieldsValue.forEach(field => {
        const name = this.taskDeclaration.filter(x => x.id === field.fieldId)[0].name;
        obj[name] = field.value;
      });
      this.data.push(obj);
    });
  }
}
