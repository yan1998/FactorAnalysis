import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ExchangeRateFactorsService } from '../services/exchange-rate-factors.service';
import { CurrencyExchangePredicationRequest } from '../models/currency-exchange-predication-request';

@Component({
  selector: 'app-prediction',
  templateUrl: './prediction.component.html',
  styleUrls: ['./prediction.component.css']
})
export class PredictionComponent implements OnInit {

  predictionResult: number = null;
  currencyExchangePredictionForm = new FormGroup({
    creditRate: new FormControl('', [Validators.required]),
    gdpIndicator: new FormControl('', [Validators.required]),
    importIndicator: new FormControl('', [Validators.required]),
    exportIndicator: new FormControl('', [Validators.required]),
    inflationIndex: new FormControl('', [Validators.required]),
    currency: new FormControl('', [Validators.required])
  });

  constructor(private _exchangeRateFactorsService: ExchangeRateFactorsService) { }

  ngOnInit() {
  }

  public predictCurrency() {
    const currency = this.currencyExchangePredictionForm.get('currency').value;
    const request: CurrencyExchangePredicationRequest = {
      creditRate: this.currencyExchangePredictionForm.get('creditRate').value,
      gdpIndicator: this.currencyExchangePredictionForm.get('gdpIndicator').value,
      importIndicator: this.currencyExchangePredictionForm.get('importIndicator').value,
      exportIndicator: this.currencyExchangePredictionForm.get('exportIndicator').value,
      inflationIndex: this.currencyExchangePredictionForm.get('inflationIndex').value
    };

    if (currency === 'USD') {
      this._exchangeRateFactorsService.predictUSDCurrencyExchange(request).subscribe(result => {
        this.predictionResult = result;
      }, error => console.error(error));
    } else {
      this._exchangeRateFactorsService.predictEURCurrencyExchange(request).subscribe(result => {
        this.predictionResult = result;
      }, error => console.error(error));
    }
  }

}
