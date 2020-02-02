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
        const href = this.serverUrl + `GetExchangeRateFactorsRange/${dateFrom.toJSON().slice(0, 10)}/${dateTo.toJSON().slice(0, 10)}`;
        return this._httpClient.get<ExchangeRateFactors[]>(href);
    }
}
