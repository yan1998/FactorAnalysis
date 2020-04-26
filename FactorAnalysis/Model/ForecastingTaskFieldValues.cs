using System.Collections.Generic;

namespace FactorAnalysis.Model
{
    public class ForecastingTaskFieldValues
    {
        public string id { get; set; }

        public List<ForecastingTaskFieldValue> FieldsValue { get; set; }
    }
}
