using System.Collections.Generic;

namespace FactorAnalysis.Model
{
    public class AlgorithmPredictionResult
    {
        public List<ForecastingTaskFieldValue> FactorValues { get; set; }

        public float Result { get; set; }
    }
}
