using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FactorAnalysis.Model.Requests
{
    public class GetPagedForecastingTaskRequest
    {
        [Required]
        public string TaskEntityName { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int PageNumber { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int PerPage { get; set; }

        public List<ForecastingTaskFieldValue> ForecastingTaskFieldValues { get; set; }
    }
}
