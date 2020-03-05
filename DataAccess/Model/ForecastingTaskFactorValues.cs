using MongoDB.Bson;
using System.Collections.Generic;

namespace DataAccess.Model
{
    public class ForecastingTaskFactorValues
    {
        public ObjectId _id { get; set; }
        public List<ForecastingTaskFactorValue> FactorsValue { get; set; }
    }

    public class ForecastingTaskFactorValue
    {
        public int FactorId { get; set; }
        public float Value { get; set; }
    }
}
