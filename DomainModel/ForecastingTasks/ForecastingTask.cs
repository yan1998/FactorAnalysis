using System.Collections.Generic;

namespace DomainModel.ForecastingTasks
{
    public class ForecastingTask
    {
        public string Name { get; set; }

        public List<ForecastingTaskFieldDeclaration> FieldsDeclaration { get; set; }

        public List<ForecastingTaskFieldValues> FactorsValues { get; set; }
    }

    public class ForecastingTaskFieldValues
    {
        public string id { get; set; }

        public List<ForecastingTaskFieldValue> FactorsValue { get; set; }
    }
}
