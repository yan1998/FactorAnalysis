using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FactorAnalysis.Model.Requests
{
    public class CreateForecastingTaskEntityRequest
    {
        public string TaskEntityName { get; set; }

        public List<ForecastingTaskFactorDeclarationCreationRequest> TaskFactorsDeclaration { get; set; }
    }

    public class ForecastingTaskFactorDeclarationCreationRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsPredicatedValue { get; set; }
    }
}
