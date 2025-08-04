using ContactMicroservice.Domain.Enums;

namespace ContactMicroservice.Application.DTOs
{
    public class ContactInfoDto
    {
        public ContactType Type { get; set; }
        public string Value { get; set; }
    }
}
