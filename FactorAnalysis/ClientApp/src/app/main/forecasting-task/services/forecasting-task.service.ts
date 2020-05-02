import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CreateForecastingTaskEntityRequest } from '../models/requests/create-forecasting-task-entity-request';
import { PagedForecastingTaskResponse } from '../models/responses/paged-forecasting-task-response';
import { LearningAlgorithm } from '../models/learning-algorithm.enum';
import { ForecastingTaskFieldValueRequest } from '../models/requests/forecasting-task-field-value-request';
import { PredictValueRequest } from '../models/requests/predict-value-request';
import { GetForecastingTaskEntitiesResponse } from '../models/responses/get-forecasting-task-entities-response';
import { UpdateForecastingTaskEntityRequest } from '../models/requests/update-forecasting-task-entity-request';
import { GetPagedForecastingTaskRequest } from '../models/requests/get-paged-forecasting-task-request';
import { GetForecastingTaskDeclarationResponse } from '../models/responses/get-forecasting-task-declaration-response';
import { AnalyzePredictionAlgorithmsResponse } from '../models/responses/analyze-prediction-algorithms-response';
import { AnalyzePredictionAlgorithmsRequest } from '../models/requests/analyze-prediction-algorithms-request';

@Injectable({
  providedIn: 'root'
})
export class ForecastingTaskService {

  private serverUrl = '../api/ForecastingTaskEntity/';

  constructor(private _httpClient: HttpClient) { }

  getForecastingTaskEntities(): Observable<GetForecastingTaskEntitiesResponse[]> {
    const href = this.serverUrl + `ForecastingTaskEntities`;
    return this._httpClient.get<GetForecastingTaskEntitiesResponse[]>(href);
  }

  createForecatingTaskEntity(creationRequest: CreateForecastingTaskEntityRequest): Observable<void> {
    const href = this.serverUrl + `ForecastingTaskEntity`;
    return this._httpClient.post<void>(href, creationRequest);
  }

  updateForecastingTaskEntity(updateRequest: UpdateForecastingTaskEntityRequest): Observable<void> {
    const href = this.serverUrl + `ForecastingTaskEntity`;
    return this._httpClient.put<void>(href, updateRequest);
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

  getForecastingTaskDeclaration(taskEntityName: string): Observable<GetForecastingTaskDeclarationResponse> {
    const href = this.serverUrl + `TaskDeclaration/${taskEntityName}`;
    return this._httpClient.get<GetForecastingTaskDeclarationResponse>(href);
  }

  getPagedForecastingTask(request: GetPagedForecastingTaskRequest): Observable<PagedForecastingTaskResponse> {
    const href = this.serverUrl + `PagedForecastingTaskEntity`;
    return this._httpClient.post<PagedForecastingTaskResponse>(href, request);
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

  analyzePredictionAlgorithms(request: AnalyzePredictionAlgorithmsRequest): Observable<AnalyzePredictionAlgorithmsResponse> {
    const href = this.serverUrl + `AnalyzePredictionAlgorithms`;
    return this._httpClient.post<AnalyzePredictionAlgorithmsResponse>(href, request);
  }

  getAllForecastingTaskValues(taskEntityName: string): Observable<PagedForecastingTaskResponse> {
    const request: GetPagedForecastingTaskRequest = {
      taskEntityName: taskEntityName,
      forecastingTaskFieldValues: [],
      pageNumber: 1,
      perPage: 2147483647
    };

    const href = this.serverUrl + `PagedForecastingTaskEntity`;
    return this._httpClient.post<PagedForecastingTaskResponse>(href, request);
  }

}
