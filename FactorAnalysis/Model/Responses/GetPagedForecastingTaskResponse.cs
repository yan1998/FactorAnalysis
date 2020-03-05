using System.Collections.Generic;

namespace FactorAnalysis.Model.Responses
{
    public class GetPagedForecastingTaskResponse
    {
        public string Name { get; set; }

        public List<ForecastingTaskFactorDeclaration> FactorsDeclaration { get; set; }

        public List<ForecastingTaskFactorValues> FactorsValues { get; set; }

        public long TotalCount { get; set; }
    }

    public class ForecastingTaskFactorDeclaration
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsPredicatedValue { get; set; }
    }

    public class ForecastingTaskFactorValues
    {
        public string id { get; set; }

        public List<Requests.ForecastingTaskFactorValue> FactorsValue { get; set; }
    }
}
