using ContactMicroservice.Domain.Entities;
using ContactMicroservice.Domain.Enums;
using ContactMicroservice.Domain.Interfaces.Repositories;
using MongoDB.Driver;

namespace ContactMicroservice.Infrastructure.Persistence.Repositories
{
    public class PersonRepository : IPersonRepository
    {
        private readonly IMongoCollection<Person> _persons;

        public PersonRepository(IMongoDatabase database)
        {
            _persons = database.GetCollection<Person>("Persons");
        }

        public async Task<List<Person>> GetAllAsync() =>
            await _persons.Find(_ => true).ToListAsync();

        public async Task<Person> GetByIdAsync(Guid id) =>
            await _persons.Find(p => p.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Person person) =>
            await _persons.InsertOneAsync(person);

        public async Task UpdateAsync(Person person) =>
       await _persons.ReplaceOneAsync(p => p.Id == person.Id, person);

        public async Task DeleteAsync(Guid id) =>
            await _persons.DeleteOneAsync(p => p.Id == id);

        public async Task<int> GetCountByLocationAsync(string location)
        {
            var filter = Builders<Person>.Filter.ElemMatch(p => p.ContactInfos,
                ci => ci.Type == ContactType.Location && ci.Value == location);

            var count = await _persons.CountDocumentsAsync(filter);
            return (int)count;
        }
    }
}
