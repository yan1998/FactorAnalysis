using System.Collections.Generic;

namespace DomainModel.ForecastingTasks
{
    public class ForecastingTaskRecord
    {
        public string Id { get; set; }

        public List<ForecastingTaskFieldValue> FieldsValue { get; set; }
    }
}
