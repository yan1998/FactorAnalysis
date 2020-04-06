import { ForecastingTaskFieldValue } from './paged-forecasting-task';
import { FieldType } from './field-type.enum';

export class CreateForecastingTaskEntityRequest {
    taskEntityName: string;
    taskFieldsDeclaration: ForecastingTaskFieldDeclarationCreationRequest[];
}

export class ForecastingTaskFieldDeclarationCreationRequest {
    name: string;
    description: string;
    type: FieldType;
}

export class ForecastingTaskFieldValueRequest {
    values: ForecastingTaskFieldValue[];
}
