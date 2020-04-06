using MongoDB.Bson;
using System.Collections.Generic;

namespace DataAccess.Model
{
    public class ForecastingTaskDeclaration
    {
        public ObjectId _id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public List<ForecastingTaskFieldDeclaration> FieldsDeclaration { get; set; }
    }

    public class ForecastingTaskFieldDeclaration
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public short Type { get; set; }
    }
}
