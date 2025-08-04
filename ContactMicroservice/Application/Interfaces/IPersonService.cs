using ContactMicroservice.Application.DTOs;
using ContactMicroservice.Domain.Entities;

namespace ContactMicroservice.Application.Interfaces
{
    public interface IPersonService
    {
        Task<Person> CreatePersonAsync(PersonDto dto);
        Task<Person> GetPersonByIdAsync(Guid id);
        Task<List<Person>> GetAllPersonsAsync();
        Task DeletePersonAsync(Guid id);
        Task<Person> AddContactInfoAsync(Guid personId, ContactInfoDto contactInfoDto);
        Task<Person> DeleteContactInfoAsync(Guid personId, Guid contactInfoId);
    }

}
