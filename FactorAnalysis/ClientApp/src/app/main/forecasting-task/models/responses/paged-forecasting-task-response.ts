import { ForecastingTaskFieldDeclaration } from '../forecasting-task-field-declaration';
import { ForecastingTaskFieldValue } from '../forecasting-task-field-value';

export class PagedForecastingTaskResponse {
    name: string;
    totalCount: number;
    fieldsValues: ForecastingTaskFieldValues[];
}

export class ForecastingTaskFieldValues {
    id: string;
    fieldsValue: ForecastingTaskFieldValue[];
}
