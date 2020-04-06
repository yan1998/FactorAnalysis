using System.Collections.Generic;

namespace FactorAnalysis.Model.Requests
{
    public class CreateForecastingTaskEntityRequest
    {
        public string TaskEntityName { get; set; }

        public List<ForecastingTaskFieldDeclarationCreationRequest> TaskFieldsDeclaration { get; set; }
    }

    public class ForecastingTaskFieldDeclarationCreationRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public FieldType Type { get; set; }
    }
}
