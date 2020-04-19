import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { AppComponent } from './app.component';
import { AppRoutingModule } from './app-routing.module';
import { CurrencyExchangePredictionModule } from './currency-exchange-prediction/currency-exchange-prediction.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ForecastingTaskModule } from './main/forecasting-task/forecasting-task.module';
import { MatDialogModule } from '@angular/material/dialog';
import { CommonModule } from '@angular/common';
import { NavMenuComponent } from './nav-menu/nav-menu.component';


@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent
  ],
  imports: [
    CommonModule,
    BrowserModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    CurrencyExchangePredictionModule,
    ForecastingTaskModule,
    HttpClientModule,
    MatDialogModule
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
