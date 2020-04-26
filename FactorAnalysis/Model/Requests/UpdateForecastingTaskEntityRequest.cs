using System.ComponentModel.DataAnnotations;

namespace FactorAnalysis.Model.Requests
{
    public class UpdateForecastingTaskEntityRequest
    {
        public string OldTaskName { get; set; }

        public string NewTaskName { get; set; }

        public string Description { get; set; }
    }
}
