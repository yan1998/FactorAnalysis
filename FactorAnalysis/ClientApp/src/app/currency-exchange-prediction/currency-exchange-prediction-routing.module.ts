import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PredictionComponent } from './prediction/prediction.component';
import { ExchangeRateFactorsDiagramComponent } from './exchange-rate-factors-diagram/exchange-rate-factors-diagram.component';
import { CurrencyExchangePredictionComponent } from './currency-exchange-prediction.component';

const routes: Routes = [
  { path: '',  component: CurrencyExchangePredictionComponent , children: [
      { path: 'prediction', component: PredictionComponent },
      { path: 'exchange-rate-factors-diagram' , component: ExchangeRateFactorsDiagramComponent }
    ]
  }
];

@NgModule({
  imports: [ RouterModule.forChild(routes) ],
  exports: [ RouterModule ]
})
export class CurrencyExchangePredictionRoutingModule {}
