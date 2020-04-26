using System.Collections.Generic;

namespace FactorAnalysis.Model.Requests
{
    public class PredictValueRequest
    {
        public List<ForecastingTaskFieldValue> Values { get; set; }
    }
}
