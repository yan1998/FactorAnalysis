import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CreateForecastingTaskEntityRequest } from '../models/requests/create-forecasting-task-entity-request';
import { PagedForecastingTask } from '../models/paged-forecasting-task';
import { LearningAlgorithm } from '../models/learning-algorithm.enum';
import { ForecastingTaskFieldValueRequest } from '../models/requests/forecasting-task-field-value-request';
import { PredictValueRequest } from '../models/requests/predict-value-request';

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

  addForecstingTaskFactorsValue(taskEntityName: string, values: ForecastingTaskFieldValueRequest): Observable<void> {
    const href = this.serverUrl + `ForecastingTaskEntity/${taskEntityName}`;
    return this._httpClient.post<void>(href, values);
  }

  getPagedForecastingTask(taskEntityName: string, pageNumber: number, perPage: number): Observable<PagedForecastingTask> {
    const href = this.serverUrl + `PagedForecastingTaskEntity/${taskEntityName}/${pageNumber}/${perPage}`;
    return this._httpClient.get<PagedForecastingTask>(href);
  }

  saveForecastingTaskEntityCsv(taskEntityName: string): Observable<Blob> {
    const href = this.serverUrl + `SaveForecastingTaskValuesCsv/${taskEntityName}`;
    return this._httpClient.get(href, { responseType: 'blob' });
  }

  uploadForecastingTaskValuesCsv(taskEntityName: string, formData: FormData): Observable<void> {
    const href = this.serverUrl + `UploadCsvFile/${taskEntityName}`;
    return this._httpClient.post<void>(href, formData);
  }

  сreateTaskEntityPredictionModel(taskEntityName: string, learningAlgorithm: LearningAlgorithm): Observable<void> {
    const href = this.serverUrl + `CreateTaskEntityPredictionModel/${taskEntityName}/${learningAlgorithm}`;
    return this._httpClient.post<void>(href, null);
  }

  predictValue(taskEntityName: string, values: PredictValueRequest): Observable<number> {
    const href = this.serverUrl + `PredictValue/${taskEntityName}`;
    return this._httpClient.post<number>(href, values);
  }
}
