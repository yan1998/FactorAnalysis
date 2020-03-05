using MongoDB.Bson;
using System.Collections.Generic;

namespace DataAccess.Model
{
    public class ForecastingTaskDeclaration
    {
        public ObjectId _id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public List<ForecastingTaskFactorDeclaration> FactorsDeclaration { get; set; }
    }

    public class ForecastingTaskFactorDeclaration
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsPredicatedValue { get; set; }
    }
}
