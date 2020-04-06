using System.Collections.Generic;

namespace FactorAnalysis.Model.Requests
{
    public class AddForecstingTaskFactorsValueRequest
    {
        public List<ForecastingTaskFieldValue> Values { get; set; }
    }

    public class ForecastingTaskFieldValue
    {
        public int FieldId { get; set; }
        public float Value { get; set; }
    }
}
