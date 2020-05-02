using System.Collections.Generic;

namespace FactorAnalysis.Model.Requests
{
    public class AnalyzePredictionAlgorithmsRequest
    {
        public string TaskEntityName { get; set; }

        public List<string> Algorithms { get; set; }
    }
}
