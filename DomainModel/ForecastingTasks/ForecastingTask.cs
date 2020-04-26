using System.Collections.Generic;

namespace DomainModel.ForecastingTasks
{
    public class ForecastingTask
    {
        public string Name { get; set; }

        public List<ForecastingTaskFieldDeclaration> FieldsDeclaration { get; set; }

        public List<ForecastingTaskFieldValues> FieldsValues { get; set; }
    }
}
