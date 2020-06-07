using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AppleShowcase.Data.Models
{
    public class User
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
    }
}