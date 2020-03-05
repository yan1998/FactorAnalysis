using System.Collections.Generic;

namespace FactorAnalysis.Model.Requests
{
    public class AddForecstingTaskFactorsValueRequest
    {
        public List<ForecastingTaskFactorValue> Values { get; set; }
    }

    public class ForecastingTaskFactorValue
    {
        public int FactorId { get; set; }
        public float Value { get; set; }
    }
}
