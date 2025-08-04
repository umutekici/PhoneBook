using ContactMicroservice.Domain.Enums;

namespace ContactMicroservice.Domain.Entities
{
    public class ContactInfo
    {
        public Guid Id { get; set; }

        public ContactType Type { get; set; }

        public string Value { get; set; }
    }
}
