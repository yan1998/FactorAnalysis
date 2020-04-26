using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FactorAnalysis.Model.Requests
{
    public class CreateForecastingTaskEntityRequest
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public List<ForecastingTaskFieldDeclarationCreationRequest> FieldsDeclaration { get; set; }
    }

    public class ForecastingTaskFieldDeclarationCreationRequest
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public FieldType Type { get; set; }
    }
}
