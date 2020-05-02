import { LearningAlgorithm } from './learning-algorithm.enum';
import { AlgorithmPredictionResult } from './algorithm-prediction-result';

export class AnalyzePredictionAlgorithmsReport {
    algorithm: LearningAlgorithm;
    elapsedTrainingTime: Date;
    elapsedPredictionTime: Date;
    averageError: number;
    results: AlgorithmPredictionResult[];
}
