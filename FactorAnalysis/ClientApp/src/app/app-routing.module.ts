import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  {
    path: '', loadChildren: './main/main.module#MainModule'
  },
  {
    path: 'currency-exchange-prediction',
    loadChildren: './currency-exchange-prediction/currency-exchange-prediction.module#CurrencyExchangePredictionModule'
  }
];

@NgModule({
  imports: [ RouterModule.forRoot(routes) ],
  exports: [ RouterModule ]
})
export class AppRoutingModule {}
