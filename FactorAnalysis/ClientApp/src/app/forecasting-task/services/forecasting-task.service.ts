import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CreateForecastingTaskEntityRequest } from '../models/create-forecasting-task-entity-request';
import { PagedForecastingTask, ForecastingTaskFactorValue } from '../models/paged-forecasting-task';

@Injectable({
  providedIn: 'root'
})
export class ForecastingTaskService {

  private serverUrl = '../api/ForecastingTaskEntity/';

  constructor(private _httpClient: HttpClient) { }

  getForecastingTaskEntitiesName(): Observable<string[]> {
    const href = this.serverUrl + `ForecastingTaskEntitiesNames`;
    return this._httpClient.get<string[]>(href);
  }

  createForecatingTaskEntity(creationRequest: CreateForecastingTaskEntityRequest): Observable<void> {
    const href = this.serverUrl + `ForecastingTaskEntity`;
    return this._httpClient.post<void>(href, creationRequest);
  }

  deleteForecastingTaskEntity(entityName: string): Observable<void> {
    const href = this.serverUrl + `ForecastingTaskEntity/${entityName}`;
    return this._httpClient.delete<void>(href);
  }

  deleteForecastingTaskEntityValues(entityName: string, id: number): Observable<void> {
    const href = this.serverUrl + `ForecastingTaskEntity/${entityName}/${id}`;
    return this._httpClient.delete<void>(href);
  }

  addForecstingTaskFactorsValue(taskEntityName: string, taskFactorValues: ForecastingTaskFactorValue[]): Observable<void> {
    const href = this.serverUrl + `ForecastingTaskEntity/${taskEntityName}`;
    return this._httpClient.post<void>(href, taskFactorValues);
  }

  getPagedForecastingTask(taskEntityName: string, pageNumber: number, perPage: number): Observable<PagedForecastingTask> {
    const href = this.serverUrl + `PagedForecastingTaskEntity/${taskEntityName}/${pageNumber}/${perPage}`;
    return this._httpClient.get<PagedForecastingTask>(href);
  }
}
