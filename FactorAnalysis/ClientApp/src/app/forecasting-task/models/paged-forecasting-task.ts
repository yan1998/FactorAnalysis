export class PagedForecastingTask {
    name: string;
    totalCountL: number;
    factorsDeclaration: ForecastingTaskFactorDeclaration[];
    FactorsValues: ForecastingTaskFactorValues[];
}

export class ForecastingTaskFactorDeclaration {
    id: number;
    name: string;
    description: string;
    isPredicatedValues: boolean;
}

export class ForecastingTaskFactorValues {
    id: string;
    factorsValue: ForecastingTaskFactorValue[];
}


export class ForecastingTaskFactorValue {
    factorId: number;
    value: number;
}
