using ContactMicroservice.Domain.Entities;

namespace ContactMicroservice.Domain.Interfaces.Repositories
{
    public interface IPersonRepository
    {
        Task<List<Person>> GetAllAsync();
        Task<Person> GetByIdAsync(Guid id);
        Task CreateAsync(Person person);
        Task UpdateAsync(Person person);
        Task DeleteAsync(Guid id);
        Task<int> GetCountByLocationAsync(string location);
    }
}
