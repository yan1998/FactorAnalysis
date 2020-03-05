using System;
using System.Collections.Generic;
using System.Text;

namespace DomainModel.ForecastingTasks
{
    public class PagedForecastingTask
    {
        public string Name { get; set; }

        public List<ForecastingTaskFactorDeclaration> FactorsDeclaration { get; set; }

        public List<ForecastingTaskFactorValues> FactorsValues { get; set; }

        public long TotalCount { get; set; }
    }
}
