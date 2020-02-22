import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PredictionComponent } from './prediction/prediction.component';
import { ExchangeRateFactorsDiagramComponent } from './exchange-rate-factors-diagram/exchange-rate-factors-diagram.component';
import { ExchangeRateFactorsService } from './services/exchange-rate-factors.service';
import { CurrencyExchangePredictionRoutingModule } from './currency-exchange-prediction-routing.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatRadioModule } from '@angular/material/radio';
import { MatNativeDateModule } from '@angular/material/core';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatButtonModule } from '@angular/material/button';
import { ChartsModule } from 'ng2-charts';
import { CurrencyExchangePredictionComponent } from './currency-exchange-prediction.component';
import { ExchangeRateFactorsTableComponent } from './exchange-rate-factors-table/exchange-rate-factors-table.component';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatMenuModule } from '@angular/material/menu';
import { MatDialogModule } from '@angular/material/dialog';
import { ConfirmationDialogComponent } from '../dialog-windows/confirmation-dialog/confirmation-dialog.component';
import { ExchangeRateFactorsDialogComponent } from './dialog-windows/exchange-rate-factors-dialog/exchange-rate-factors-dialog.component';

@NgModule({
  declarations: [
    CurrencyExchangePredictionComponent,
    PredictionComponent,
    ExchangeRateFactorsDiagramComponent,
    ExchangeRateFactorsTableComponent,
    ConfirmationDialogComponent,
    ExchangeRateFactorsDialogComponent
  ],
  imports: [
    CommonModule,
    CurrencyExchangePredictionRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatRadioModule,
    MatInputModule,
    MatSelectModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatButtonModule,
    ChartsModule,
    MatTableModule,
    MatPaginatorModule,
    MatProgressSpinnerModule,
    MatDialogModule,
    MatMenuModule
  ],
  providers: [
    ExchangeRateFactorsService
  ],
  entryComponents: [
    ConfirmationDialogComponent,
    ExchangeRateFactorsDialogComponent
  ]
})
export class CurrencyExchangePredictionModule { }
