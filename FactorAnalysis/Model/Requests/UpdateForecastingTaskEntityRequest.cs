using System.ComponentModel.DataAnnotations;

namespace FactorAnalysis.Model.Requests
{
    public class UpdateForecastingTaskEntityRequest
    {
        [Required]
        public string OldTaskName { get; set; }

        [Required]
        public string NewTaskName { get; set; }

        [Required]
        public string Description { get; set; }
    }
}
