using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FactorAnalysis.Model.Requests
{
    public class GetPagedForecastingTaskRequest
    {
        public string TaskEntityName { get; set; }

        public int PageNumber { get; set; }

        public int PerPage { get; set; }

        public List<ForecastingTaskFieldValue> ForecastingTaskFieldValues { get; set; }
    }
}
