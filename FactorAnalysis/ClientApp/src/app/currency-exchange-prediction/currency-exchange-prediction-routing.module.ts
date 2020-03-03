import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PredictionComponent } from './prediction/prediction.component';
import { ExchangeRateFactorsDiagramComponent } from './exchange-rate-factors-diagram/exchange-rate-factors-diagram.component';
import { CurrencyExchangePredictionComponent } from './currency-exchange-prediction.component';
import { ExchangeRateFactorsTableComponent } from './exchange-rate-factors-table/exchange-rate-factors-table.component';

const routes: Routes = [
  { path: '',  component: CurrencyExchangePredictionComponent , children: [
      { path: 'prediction', component: PredictionComponent },
      { path: 'exchange-rate-factors-diagram' , component: ExchangeRateFactorsDiagramComponent },
      { path: 'exchange-rate-factors-table' , component: ExchangeRateFactorsTableComponent }
    ]
  }
];

@NgModule({
  imports: [ RouterModule.forChild(routes) ],
  exports: [ RouterModule ]
})
export class CurrencyExchangePredictionRoutingModule {}
