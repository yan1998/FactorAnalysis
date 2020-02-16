import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';

const routes: Routes = [
  { path: '', component: HomeComponent, pathMatch: 'full' },
  { path: 'currency-exchange-prediction',  loadChildren: './currency-exchange-prediction/currency-exchange-prediction.module#CurrencyExchangePredictionModule' },
  { path: '**', redirectTo: '/', pathMatch: 'prefix' }
];

@NgModule({
  imports: [ RouterModule.forRoot(routes) ],
  exports: [ RouterModule ]
})
export class AppRoutingModule {}
