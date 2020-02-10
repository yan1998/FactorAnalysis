import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient, HttpHeaderResponse, HttpHeaders } from '@angular/common/http';
import { ExchangeRateFactors } from '../models/exchange-rate-factors';

@Injectable({
    providedIn: 'root'
})
export class ExchangeRateFactorsService {

    private serverUrl = '../api/ExchangeRateFactors/';

    constructor(private _httpClient: HttpClient) { }

    getExchangeRateFactorsRange(dateFrom: Date, dateTo: Date): Observable<ExchangeRateFactors[]> {
        const href = this.serverUrl + `GetExchangeRateFactorsRange/${dateFrom.getFullYear()}-${dateFrom.getMonth() + 1}-${dateFrom.getDate()}/${dateTo.getFullYear()}-${dateTo.getMonth() + 1}-${dateTo.getDate()}`;
        return this._httpClient.get<ExchangeRateFactors[]>(href);
    }
}
