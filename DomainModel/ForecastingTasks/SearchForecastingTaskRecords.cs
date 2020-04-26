using System.Collections.Generic;

namespace DomainModel.ForecastingTasks
{
    public class SearchForecastingTaskRecords
    {
        public string TaskEntityName { get; set; }

        public int PageNumber { get; set; }

        public int PerPage { get; set; }

        public List<ForecastingTaskFieldValue> Filters { get; set; }
    }
}
