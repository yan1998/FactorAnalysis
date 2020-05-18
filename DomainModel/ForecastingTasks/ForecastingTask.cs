using System.Collections.Generic;

namespace DomainModel.ForecastingTasks
{
    public class ForecastingTask
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public List<ForecastingTaskFieldDeclaration> FieldsDeclaration { get; set; }

        public List<ForecastingTaskRecord> Records { get; set; }
    }
}
