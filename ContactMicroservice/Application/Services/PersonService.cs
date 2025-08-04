using ContactMicroservice.Application.DTOs;
using ContactMicroservice.Application.Interfaces;
using ContactMicroservice.Domain.Entities;
using ContactMicroservice.Domain.Interfaces.Repositories;

namespace ContactMicroservice.Application.Services
{
    public class PersonService : IPersonService
    {
        private readonly IPersonRepository _personRepository;

        public PersonService(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        public async Task<Person> CreatePersonAsync(PersonDto dto)
        {
            var person = new Person
            {
                Id = Guid.NewGuid(),
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Company = dto.Company,
                ContactInfos = dto.ContactInfos?.Select(c => new ContactInfo
                {
                    Id = Guid.NewGuid(),
                    Type = c.Type,
                    Value = c.Value
                }).ToList() ?? new List<ContactInfo>()
            };

            await _personRepository.CreateAsync(person);
            return person;
        }

        public Task<Person> GetPersonByIdAsync(Guid id) => _personRepository.GetByIdAsync(id);

        public Task<List<Person>> GetAllPersonsAsync() => _personRepository.GetAllAsync();

        public Task DeletePersonAsync(Guid id) => _personRepository.DeleteAsync(id);

        public async Task<Person?> AddContactInfoAsync(Guid personId, ContactInfoDto contactInfoDto)
        {
            var person = await _personRepository.GetByIdAsync(personId);
            if (person == null)
                return null;

            var newContactInfo = new ContactInfo
            {
                Id = Guid.NewGuid(),
                Type = contactInfoDto.Type,
                Value = contactInfoDto.Value
            };

            if (person.ContactInfos == null)
                person.ContactInfos = new List<ContactInfo>();

            person.ContactInfos.Add(newContactInfo);

            await _personRepository.UpdateAsync(person);
            return person;
        }

        public async Task<Person> DeleteContactInfoAsync(Guid personId, Guid contactInfoId)
        {
            var person = await _personRepository.GetByIdAsync(personId);
            if (person == null || person.ContactInfos == null)
                return null;

            var contactInfo = person.ContactInfos.FirstOrDefault(c => c.Id == contactInfoId);
            if (contactInfo == null)
                return null;

            person.ContactInfos.Remove(contactInfo);
            await _personRepository.UpdateAsync(person);

            return person;
        }
    }
}
