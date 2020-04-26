using MongoDB.Bson;
using System.Collections.Generic;

namespace DataAccess.Model
{
    public class ForecastingTaskFieldValues
    {
        public ObjectId _id { get; set; }

        public List<ForecastingTaskFieldValue> FieldsValue { get; set; }
    }
}
