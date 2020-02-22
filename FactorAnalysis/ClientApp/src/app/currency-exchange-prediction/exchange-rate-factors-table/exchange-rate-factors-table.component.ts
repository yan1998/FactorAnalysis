import { Component, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { merge, Observable, of as observableOf } from 'rxjs';
import { catchError, map, startWith, switchMap } from 'rxjs/operators';
import { ExchangeRateFactorsService } from '../services/exchange-rate-factors.service';
import { ExchangeRateFactors } from '../models/exchange-rate-factors';
import { MatDialog } from '@angular/material/dialog';
import { ConfirmationDialogComponent } from 'src/app/dialog-windows/confirmation-dialog/confirmation-dialog.component';
import { ExchangeRateFactorsDialogComponent } from '../dialog-windows/exchange-rate-factors-dialog/exchange-rate-factors-dialog.component';

@Component({
  selector: 'app-exchange-rate-factors-table',
  templateUrl: './exchange-rate-factors-table.component.html',
  styleUrls: ['./exchange-rate-factors-table.component.css']
})
export class ExchangeRateFactorsTableComponent implements OnInit, AfterViewInit  {

  @ViewChild(MatPaginator, {static: false}) paginator: MatPaginator;
  isLoadingResults: boolean;
  resultsLength: number;
  data: ExchangeRateFactors[];
  displayedColumns: string[] = ['id', 'date', 'exchangeRateUSD', 'exchangeRateEUR', 'creditRate', 'gdpIndicator', 'importIndicator', 'exportIndicator', 'inflationIndex', 'actions'];

  constructor(private _exchangeRateFactorsService: ExchangeRateFactorsService,
    private dialog: MatDialog) { }

  ngOnInit() {
    this.isLoadingResults = true;
    this.data = [];
  }

  ngAfterViewInit() {
    merge(this.paginator.page).pipe(
      startWith({}),
      switchMap(() => {
        this.isLoadingResults = true;
        return this._exchangeRateFactorsService.getPagedExchangeRateFactors(this.paginator.pageIndex + 1, this.paginator.pageSize);
      }),
      map(data => {
        // Flip flag to show that loading has finished.
        this.isLoadingResults = false;
        this.resultsLength = data.totalAmount;
        return data.exchangeRateFactors;
      }),
      catchError(() => {
        this.isLoadingResults = false;
        return observableOf([]);
      })
    ).subscribe(data => this.data = data);
  }

  addRecord() {
    const dialogRef = this.dialog.open(ExchangeRateFactorsDialogComponent, {
      width: '500px',
      data: {
        mode: 'Add'
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this._exchangeRateFactorsService.createExchangeRateFactors(result).subscribe(() => {
          this.paginator.firstPage();
        }, error => console.error(error));
      }
    });
  }

  editRecord(row: ExchangeRateFactors) {
    const dialogRef = this.dialog.open(ExchangeRateFactorsDialogComponent, {
      width: '500px',
      data: {
        mode: 'Edit',
        entity: row
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this._exchangeRateFactorsService.updateExchangeRateFactors(result.id, result).subscribe(() => {
          this.paginator.firstPage();
        }, error => console.error(error));
      }
    });
  }

  removeRecord(id: number) {
    const dialogRef = this.dialog.open(ConfirmationDialogComponent, {
      width: '300px',
      data: 'Вы действительно хотите удалить запись?'
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this._exchangeRateFactorsService.removeExchangeRateFactors(id).subscribe(() => {
          this.paginator.firstPage();
        }, error => console.error(error));
      }
    });
  }
}
