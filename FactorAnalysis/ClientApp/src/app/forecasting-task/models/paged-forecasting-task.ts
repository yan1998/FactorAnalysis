export class PagedForecastingTask {
    name: string;
    totalCount: number;
    fieldsDeclaration: ForecastingTaskFieldDeclaration[];
    fieldsValues: ForecastingTaskFieldValues[];
}

export class ForecastingTaskFieldDeclaration {
    id: number;
    name: string;
    description: string;
    isPredicatedValues: boolean;
}

export class ForecastingTaskFieldValues {
    id: string;
    fieldsValue: ForecastingTaskFieldValue[];
}


export class ForecastingTaskFieldValue {
    fieldId: number;
    value: number;
}
