using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ContactMicroservice.Domain.Entities
{
    public class Person
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Company { get; set; }

        public List<ContactInfo> ContactInfos { get; set; }
    }
}
