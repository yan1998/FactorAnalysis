import { ForecastingTaskFieldValue } from '../forecasting-task-field-value';

export class GetPagedForecastingTaskRequest {
    taskEntityName: string;
    pageNumber: number;
    perPage: number;
    forecastingTaskFieldValues: ForecastingTaskFieldValue[];
}
