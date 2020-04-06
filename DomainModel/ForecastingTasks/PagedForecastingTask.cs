using System;
using System.Collections.Generic;
using System.Text;

namespace DomainModel.ForecastingTasks
{
    public class PagedForecastingTask
    {
        public string Name { get; set; }

        public List<ForecastingTaskFieldDeclaration> FieldsDeclaration { get; set; }

        public List<ForecastingTaskFieldValues> FieldsValues { get; set; }

        public long TotalCount { get; set; }
    }
}
