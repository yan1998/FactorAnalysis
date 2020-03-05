import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { ExchangeRateFactors } from '../models/exchange-rate-factors';
import { CurrencyExchangePredicationRequest } from '../models/currency-exchange-predication-request';
import { PagedExchangeRateFactors } from '../models/paged-exchange-rate-factors';

@Injectable({
    providedIn: 'root'
})
export class ExchangeRateFactorsService {

    private serverUrl = '../api/ExchangeRateFactors/';

    constructor(private _httpClient: HttpClient) { }

    getExchangeRateFactorsRange(dateFrom: Date, dateTo: Date): Observable<ExchangeRateFactors[]> {
        const href = this.serverUrl + `ExchangeRateFactorsRange/${dateFrom.getFullYear()}-${dateFrom.getMonth() + 1}-${dateFrom.getDate()}/${dateTo.getFullYear()}-${dateTo.getMonth() + 1}-${dateTo.getDate()}`;
        return this._httpClient.get<ExchangeRateFactors[]>(href);
    }

    predictEURCurrencyExchange(request: CurrencyExchangePredicationRequest): Observable<number> {
        const href = this.serverUrl + `PredictEURCurrencyExchange/${request.creditRate}/${request.gdpIndicator}/${request.importIndicator}/${request.exportIndicator}/${request.inflationIndex}`;
        return this._httpClient.get<number>(href);
    }

    predictUSDCurrencyExchange(request: CurrencyExchangePredicationRequest): Observable<number> {
        const href = this.serverUrl + `PredictUSDCurrencyExchange/${request.creditRate}/${request.gdpIndicator}/${request.importIndicator}/${request.exportIndicator}/${request.inflationIndex}`;
        return this._httpClient.get<number>(href);
    }

    getPagedExchangeRateFactors(pageNumber: number, perPage: number): Observable<PagedExchangeRateFactors> {
        const href = this.serverUrl + `PagedExchangeRateFactors/${pageNumber}/${perPage}`;
        return this._httpClient.get<PagedExchangeRateFactors>(href);
    }

    createExchangeRateFactors(factors: ExchangeRateFactors): Observable<void>  {
        const href = this.serverUrl + `ExchangeRateFactors`;
        return this._httpClient.post<void>(href, factors);
    }

    updateExchangeRateFactors(id: number, factors: ExchangeRateFactors): Observable<void>  {
        const href = this.serverUrl + `ExchangeRateFactors/${id}`;
        return this._httpClient.put<void>(href, factors);
    }

    removeExchangeRateFactors(id: number): Observable<void> {
        const href = this.serverUrl + `ExchangeRateFactors/${id}`;
        return this._httpClient.delete<void>(href);
    }
}
