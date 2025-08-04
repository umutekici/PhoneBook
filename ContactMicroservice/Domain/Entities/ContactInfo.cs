using ContactMicroservice.Domain.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ContactMicroservice.Domain.Entities
{
    public class ContactInfo
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }

        public ContactType Type { get; set; }

        public string Value { get; set; }
    }
}
