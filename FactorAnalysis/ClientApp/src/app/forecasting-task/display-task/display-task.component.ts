import { Component, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ForecastingTaskService } from '../services/forecasting-task.service';
import { PagedForecastingTask, ForecastingTaskFactorValue } from '../models/paged-forecasting-task';
import { MatPaginator } from '@angular/material/paginator';
import { merge, of as observableOf } from 'rxjs';
import { startWith, switchMap, map, catchError } from 'rxjs/operators';
import { MatDialog } from '@angular/material/dialog';
import { ConfirmationDialogComponent } from 'src/app/dialog-windows/confirmation-dialog/confirmation-dialog.component';
import { AddDorecastingTaskDataDialogComponent } from '../dialog-windows/add-dorecasting-task-data-dialog/add-dorecasting-task-data-dialog.component';
import { ForecastingTaskFactorValueRequest } from '../models/create-forecasting-task-entity-request';

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

  constructor(private _forecastingTaskService: ForecastingTaskService,
    private route: ActivatedRoute,
    private dialog: MatDialog) { }

  ngOnInit() {
    this.isLoadingResults = true;
    this.data = [];
    this.task = {
      factorsDeclaration: [],
      factorsValues: [],
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
    this.task.factorsDeclaration.forEach(element => {
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
        const request: ForecastingTaskFactorValueRequest = {
          values: result
        };
        this._forecastingTaskService.addForecstingTaskFactorsValue(this.name, request).subscribe(() => {
          this.paginator.pageIndex = 1;
          this.paginator.firstPage();
        }, error => console.error(error));
      }
    });
  }

  private createArray() {
    this.displayedColumns = [];
    this.data = [];

    this.task.factorsDeclaration.forEach(element => {
      this.displayedColumns.push(element.name);
    });
    this.displayedColumns.push('delete');

    this.task.factorsValues.forEach(element => {
      const obj = {
        id: element.id
      };
      element.factorsValue.forEach(factor => {
        obj[this.displayedColumns[factor.factorId]] = factor.value;
      });
      this.data.push(obj);
    });
  }
}
