using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Catalog.API.Entities
{
    public class Category
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string CategoryId { get; set; }
        [BsonElement("Name")]
        public string Name { get; set; }
        [BsonIgnore]
        public List<Product> Products { get; set; }
    }
}
