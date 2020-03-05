using System.Collections.Generic;

namespace DomainModel.ForecastingTasks
{
    public class ForecastingTask
    {
        public string Name { get; set; }

        public List<ForecastingTaskFactorDeclaration> FactorsDeclaration { get; set; }

        public List<ForecastingTaskFactorValues> FactorsValues { get; set; }
    }

    public class ForecastingTaskFactorValues
    {
        public string id { get; set; }

        public List<ForecastingTaskFactorValue> FactorsValue { get; set; }
    }
}
