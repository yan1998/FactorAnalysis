import { ForecastingTaskFieldValue } from "./paged-forecasting-task";

export class CreateForecastingTaskEntityRequest {
    taskEntityName: string;
    taskFieldsDeclaration: ForecastingTaskFieldDeclarationCreationRequest[];
}

export class ForecastingTaskFieldDeclarationCreationRequest {
    name: string;
    description: string;
    isPredicatedValue: boolean;
}

export class ForecastingTaskFieldValueRequest {
    values: ForecastingTaskFieldValue[];
}
