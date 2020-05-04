using System;
using System.Collections.Generic;

namespace DomainModel.ForecastingTasks
{
    public class AlgorithmPredictionReport
    {
        public LearningAlgorithm Algorithm { get; set; }
        
        public TimeSpan ElapsedTrainingTime { get; set; }

        public TimeSpan ElapsedPredictionTime { get; set; }

        public double AverageError { get; set; }

        public List<AlgorithmPredictionResult> Results { get; set; }
    }

    public class AlgorithmPredictionResult
    {
        public List<ForecastingTaskFieldValue> FactorValues { get; set; }

        public float Result { get; set; }
    }
}
