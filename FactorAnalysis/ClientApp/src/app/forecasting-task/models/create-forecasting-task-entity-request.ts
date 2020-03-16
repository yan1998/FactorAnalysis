export class CreateForecastingTaskEntityRequest {
    taskEntityName: string;
    taskFactorsDeclaration: ForecastingTaskFactorDeclarationCreationRequest[];
}

export class ForecastingTaskFactorDeclarationCreationRequest {
    name: string;
    description: string;
    isPredicatedValue: boolean;
}
