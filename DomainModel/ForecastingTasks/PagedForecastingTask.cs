using System.Collections.Generic;

namespace DomainModel.ForecastingTasks
{
    public class PagedForecastingTask
    {
        public string Name { get; set; }

        public List<ForecastingTaskFieldValues> FieldsValues { get; set; }

        public long TotalCount { get; set; }
    }
}
