using System.Collections.Generic;

namespace FactorAnalysis.Model.Responses
{
    public class GetPagedForecastingTaskResponse
    {
        public string Name { get; set; }

        public List<ForecastingTaskFieldDeclaration> FieldsDeclaration { get; set; }

        public List<ForecastingTaskFieldValues> FactorsValues { get; set; }

        public long TotalCount { get; set; }
    }

    public class ForecastingTaskFieldDeclaration
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsPredicatedValue { get; set; }
    }

    public class ForecastingTaskFieldValues
    {
        public string id { get; set; }

        public List<Requests.ForecastingTaskFieldValue> FactorsValue { get; set; }
    }
}
