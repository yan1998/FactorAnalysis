using System.ComponentModel.DataAnnotations;

namespace FactorAnalysis.Model.Requests
{
    public class UpdateForecastingTaskEntityRequest
    {
        [Required]
        [MaxLength(50)]
        public string OldTaskName { get; set; }

        [Required]
        [MaxLength(50)]
        public string NewTaskName { get; set; }

        [MaxLength(200)]
        public string Description { get; set; }
    }
}
