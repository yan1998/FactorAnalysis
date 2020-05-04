import { LearningAlgorithm } from '../learning-algorithm.enum';

export class AnalyzePredictionAlgorithmsRequest {
    taskEntityName: string;
    algorithms: string[];
}
