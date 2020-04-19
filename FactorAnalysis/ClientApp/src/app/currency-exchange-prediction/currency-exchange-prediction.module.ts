import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PredictionComponent } from './prediction/prediction.component';
import { ExchangeRateFactorsDiagramComponent } from './exchange-rate-factors-diagram/exchange-rate-factors-diagram.component';
import { ExchangeRateFactorsService } from './services/exchange-rate-factors.service';
import { CurrencyExchangePredictionRoutingModule } from './currency-exchange-prediction-routing.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatRadioModule } from '@angular/material/radio';
import { MatNativeDateModule } from '@angular/material/core';
import { MatSelectModule } from '@angular/material/select';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { ChartsModule } from 'ng2-charts';
import { CurrencyExchangePredictionComponent } from './currency-exchange-prediction.component';
import { ExchangeRateFactorsTableComponent } from './exchange-rate-factors-table/exchange-rate-factors-table.component';
import { MatMenuModule } from '@angular/material/menu';
import { ExchangeRateFactorsDialogComponent } from './dialog-windows/exchange-rate-factors-dialog/exchange-rate-factors-dialog.component';
import { MatButtonModule } from '@angular/material/button';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatDialogModule } from '@angular/material/dialog';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { ConfirmationDialogComponent } from '../main/dialog-windows/confirmation-dialog/confirmation-dialog.component';


@NgModule({
  declarations: [
    CurrencyExchangePredictionComponent,
    PredictionComponent,
    ExchangeRateFactorsDiagramComponent,
    ExchangeRateFactorsTableComponent,
    ExchangeRateFactorsDialogComponent,
    ConfirmationDialogComponent
  ],
  imports: [
    CommonModule,
    CurrencyExchangePredictionRoutingModule,
    FormsModule,
    ChartsModule,
    ReactiveFormsModule,
    MatRadioModule,
    MatSelectModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatMenuModule,
    MatButtonModule,
    MatTableModule,
    MatPaginatorModule,
    MatProgressSpinnerModule,
    MatDialogModule,
    MatInputModule,
    MatFormFieldModule
  ],
  entryComponents: [
    ExchangeRateFactorsDialogComponent,
    ConfirmationDialogComponent
  ],
  providers: [
    ExchangeRateFactorsService
  ]
})
export class CurrencyExchangePredictionModule { }
