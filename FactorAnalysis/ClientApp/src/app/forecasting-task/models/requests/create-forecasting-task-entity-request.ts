import { ForecastingTaskFieldDeclaration } from '../forecasting-task-field-declaration';
import { ForecastingTaskFieldValue } from '../forecasting-task-field-value';

export class CreateForecastingTaskEntityRequest {
    taskEntityName: string;
    taskFieldsDeclaration: ForecastingTaskFieldDeclaration[];
}
