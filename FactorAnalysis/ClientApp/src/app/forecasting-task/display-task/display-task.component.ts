import { Component, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ForecastingTaskService } from '../services/forecasting-task.service';
import { PagedForecastingTask } from '../models/paged-forecasting-task';
import { MatPaginator } from '@angular/material/paginator';
import { merge, of as observableOf } from 'rxjs';
import { startWith, switchMap, map, catchError } from 'rxjs/operators';
import { MatDialog } from '@angular/material/dialog';
import { ConfirmationDialogComponent } from 'src/app/dialog-windows/confirmation-dialog/confirmation-dialog.component';
import { AddDorecastingTaskDataDialogComponent } from '../dialog-windows/add-dorecasting-task-data-dialog/add-dorecasting-task-data-dialog.component';
import { ForecastingTaskFieldValueRequest } from '../models/requests/create-forecasting-task-entity-request';
import { FileDownloaderService } from '../services/file-downloader.service';

@Component({
  selector: 'app-display-task',
  templateUrl: './display-task.component.html',
  styleUrls: ['./display-task.component.css']
})
export class DisplayTaskComponent implements OnInit, AfterViewInit {

  @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
  name: string;
  task: PagedForecastingTask;
  resultsLength: number;
  displayedColumns: string[];
  isLoadingResults: boolean;
  data: any;
  isCsvUploading: boolean;
  isCsvDownloading: boolean;
  isModelCreating: boolean;

  constructor(private _forecastingTaskService: ForecastingTaskService,
    private _fileDownloaderService: FileDownloaderService,
    private route: ActivatedRoute,
    private dialog: MatDialog) { }

  ngOnInit() {
    this.isLoadingResults = true;
    this.data = [];
    this.task = {
      fieldsDeclaration: [],
      fieldsValues: [],
      name: '',
      totalCount: 0
    };
    this.displayedColumns = [];
    this.route.params.subscribe(params => {
      this.name = params['name'];
    });
  }

  ngAfterViewInit() {
    merge(this.paginator.page).pipe(
      startWith({}),
      switchMap(() => {
        this.isLoadingResults = true;
        return this._forecastingTaskService.getPagedForecastingTask(this.name, this.paginator.pageIndex + 1, this.paginator.pageSize);
      }),
      map(data => {
        return data;
      }),
      catchError(() => {
        this.isLoadingResults = false;
        return observableOf([]);
      })
    ).subscribe(task => {
      this.task = task as PagedForecastingTask;
      this.resultsLength = this.task.totalCount;
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
        }, error => console.error(error));
      }
    });
  }

  addForecastingTaskData() {
    const factors = [];
    this.task.fieldsDeclaration.forEach(element => {
      factors.push({
        id: element.id,
        name: element.name
      });
    });

    const dialogRef = this.dialog.open(AddDorecastingTaskDataDialogComponent, {
      width: '300px',
      data: factors
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        const request: ForecastingTaskFieldValueRequest = {
          values: result
        };
        this._forecastingTaskService.addForecstingTaskFactorsValue(this.name, request).subscribe(() => {
          this.paginator.pageIndex = 1;
          this.paginator.firstPage();
        }, error => {
          console.error(error);
          alert('error');
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
      console.log(error);
      alert('error');
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
      console.log(error);
      alert('error');
    });
  }

  createPredictionModel() {
    this.isModelCreating = true;
    this._forecastingTaskService.сreateTaskEntityPredictionModel(this.name).subscribe(() => {
      alert('done!');
      this.isModelCreating = false;
    }, error => {
      console.log(error);
      alert('error!');
      this.isModelCreating = false;
    });
  }

  private createArray() {
    this.displayedColumns = [];
    this.data = [];

    this.task.fieldsDeclaration.forEach(element => {
      this.displayedColumns.push(element.name);
    });
    this.displayedColumns.push('delete');

    this.task.fieldsValues.forEach(element => {
      const obj = {
        id: element.id
      };
      element.fieldsValue.forEach(field => {
        obj[this.displayedColumns[field.fieldId]] = field.value;
      });
      this.data.push(obj);
    });
  }
}
