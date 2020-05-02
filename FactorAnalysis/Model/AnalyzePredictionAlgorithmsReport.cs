using DomainModel.ForecastingTasks;
using System;
using System.Collections.Generic;

namespace FactorAnalysis.Model
{
    public class AnalyzePredictionAlgorithmsReport
    {
        public LearningAlgorithm Algorithm { get; set; }

        public TimeSpan ElapsedTrainingTime { get; set; }

        public TimeSpan ElapsedPredictionTime { get; set; }

        public double AverageError { get; set; }

        public List<AlgorithmPredictionResult> Results { get; set; }
    }
}
