using System.Collections.Generic;

namespace FactorAnalysis.Model.Requests
{
    public class AddForecstingTaskFactorsValueRequest
    {
        public List<ForecastingTaskFieldValue> Values { get; set; }
    }
}
