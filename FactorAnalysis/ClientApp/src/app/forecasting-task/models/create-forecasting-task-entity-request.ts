import { ForecastingTaskFactorValue } from "./paged-forecasting-task";

export class CreateForecastingTaskEntityRequest {
    taskEntityName: string;
    taskFactorsDeclaration: ForecastingTaskFactorDeclarationCreationRequest[];
}

export class ForecastingTaskFactorDeclarationCreationRequest {
    name: string;
    description: string;
    isPredicatedValue: boolean;
}

export class ForecastingTaskFactorValueRequest {
    values: ForecastingTaskFactorValue[];
}
