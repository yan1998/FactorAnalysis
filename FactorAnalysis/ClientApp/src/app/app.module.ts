import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { AppRoutingModule } from './app-routing.module';
import { CurrencyExchangePredictionModule } from './currency-exchange-prediction/currency-exchange-prediction.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ForecastingTaskModule } from './forecasting-task/forecasting-task.module';
import { MatDialogModule } from '@angular/material/dialog';
import { CommonModule } from '@angular/common';


@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent
  ],
  imports: [
    CommonModule,
    CurrencyExchangePredictionModule,
    ForecastingTaskModule,
    BrowserModule,
    BrowserAnimationsModule,
    HttpClientModule,
    AppRoutingModule,
    MatDialogModule
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
