using System.Collections.Generic;

namespace FactorAnalysis.Model.Responses
{
    public class GetPagedForecastingTaskResponse
    {
        public string Name { get; set; }

        public List<ForecastingTaskFieldValues> FieldsValues { get; set; }

        public long TotalCount { get; set; }
    }
}
