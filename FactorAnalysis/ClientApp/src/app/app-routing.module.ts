import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';

const routes: Routes = [
  { path: '', redirectTo: '/home', pathMatch: 'full' },
  { path: 'home', component: HomeComponent },
  { path: 'currency-exchange-prediction', loadChildren: './currency-exchange-prediction/currency-exchange-prediction.module#CurrencyExchangePredictionModule' },
  { path: '**', redirectTo: '/home', pathMatch: 'prefix' }
];

@NgModule({
  imports: [ RouterModule.forRoot(routes) ],
  exports: [ RouterModule ]
})
export class AppRoutingModule {}
