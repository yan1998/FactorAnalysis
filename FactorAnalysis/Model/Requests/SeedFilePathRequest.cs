using System.ComponentModel.DataAnnotations;

namespace FactorAnalysis.Model.Requests
{
    public class SeedFilePathRequest
    {
        [Required]
        public string FilePath { get; set; }
    }
}
