import { ForecastingTaskFieldDeclaration } from '../forecasting-task-field-declaration';

export class CreateForecastingTaskEntityRequest {
    name: string;
    description: string;
    fieldsDeclaration: ForecastingTaskFieldDeclaration[];
}
