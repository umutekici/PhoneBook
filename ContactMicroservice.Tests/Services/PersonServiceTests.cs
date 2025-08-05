using ContactMicroservice.Application.DTOs;
using ContactMicroservice.Application.Services;
using ContactMicroservice.Domain.Entities;
using ContactMicroservice.Domain.Enums;
using ContactMicroservice.Domain.Interfaces.Repositories;
using Moq;

namespace ContactMicroservice.Tests.Services
{
    public class PersonServiceTests
    {
        private readonly Mock<IPersonRepository> _personRepositoryMock;
        private readonly PersonService _personService;

        public PersonServiceTests()
        {
            _personRepositoryMock = new Mock<IPersonRepository>();
            _personService = new PersonService(_personRepositoryMock.Object);
        }

        [Fact]
        public async Task CreatePersonAsync_ShouldCreateAndReturnPerson()
        {
            var dto = new PersonDto
            {
                FirstName = "Umut",
                LastName = "Ekici",
                Company = "MyCompany",
                ContactInfos = new List<ContactInfoDto>
                {
                    new ContactInfoDto { Type = ContactType.Email, Value = "umut@example.com" }
                }
            };

            var result = await _personService.CreatePersonAsync(dto);

            Assert.NotNull(result);
            Assert.Equal(dto.FirstName, result.FirstName);
            Assert.Equal(dto.LastName, result.LastName);
            Assert.Equal(dto.Company, result.Company);
            Assert.NotEmpty(result.ContactInfos);
            Assert.Equal(dto.ContactInfos.First().Type, result.ContactInfos.First().Type);
            Assert.Equal(dto.ContactInfos.First().Value, result.ContactInfos.First().Value);

            _personRepositoryMock.Verify(r => r.CreateAsync(It.Is<Person>(p => p.Id == result.Id)), Times.Once);
        }

        [Fact]
        public async Task GetPersonByIdAsync_ShouldReturnPerson_WhenExists()
        {
            var personId = Guid.NewGuid();
            var person = new Person { Id = personId, FirstName = "Umut" };

            _personRepositoryMock.Setup(r => r.GetByIdAsync(personId)).ReturnsAsync(person);

            var result = await _personService.GetPersonByIdAsync(personId);

            Assert.NotNull(result);
            Assert.Equal(personId, result.Id);
            Assert.Equal("Umut", result.FirstName);
        }

        [Fact]
        public async Task GetAllPersonsAsync_ShouldReturnList()
        {
            var persons = new List<Person>
            {
                new Person { Id = Guid.NewGuid(), FirstName = "A" },
                new Person { Id = Guid.NewGuid(), FirstName = "B" }
            };

            _personRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(persons);

            var result = await _personService.GetAllPersonsAsync();


            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task DeletePersonAsync_ShouldCallRepositoryDelete()
        {
            var personId = Guid.NewGuid();

            await _personService.DeletePersonAsync(personId);

            _personRepositoryMock.Verify(r => r.DeleteAsync(personId), Times.Once);
        }

        [Fact]
        public async Task AddContactInfoAsync_ShouldAddContactInfo_WhenPersonExists()
        {
            var personId = Guid.NewGuid();
            var person = new Person { Id = personId, ContactInfos = new List<ContactInfo>() };

            _personRepositoryMock.Setup(r => r.GetByIdAsync(personId)).ReturnsAsync(person);

            var contactInfoDto = new ContactInfoDto { Type = ContactType.Phone, Value = "555-1234" };

            var result = await _personService.AddContactInfoAsync(personId, contactInfoDto);

            Assert.NotNull(result);
            Assert.Single(result.ContactInfos);
            Assert.Equal(contactInfoDto.Type, result.ContactInfos.First().Type);
            Assert.Equal(contactInfoDto.Value, result.ContactInfos.First().Value);

            _personRepositoryMock.Verify(r => r.UpdateAsync(It.Is<Person>(p => p.Id == personId && p.ContactInfos.Count == 1)), Times.Once);
        }

        [Fact]
        public async Task AddContactInfoAsync_ShouldReturnNull_WhenPersonDoesNotExist()
        {
            var personId = Guid.NewGuid();
            _personRepositoryMock.Setup(r => r.GetByIdAsync(personId)).ReturnsAsync((Person)null);

            var contactInfoDto = new ContactInfoDto { Type = ContactType.Phone, Value = "555-1234" };

            var result = await _personService.AddContactInfoAsync(personId, contactInfoDto);

            Assert.Null(result);
        }

        [Fact]
        public async Task DeleteContactInfoAsync_ShouldRemoveContactInfo_WhenExists()
        {
            var personId = Guid.NewGuid();
            var contactInfoId = Guid.NewGuid();

            var person = new Person
            {
                Id = personId,
                ContactInfos = new List<ContactInfo>
                {
                    new ContactInfo { Id = contactInfoId, Type = ContactType.Email, Value = "a@b.com" },
                    new ContactInfo { Id = Guid.NewGuid(), Type = ContactType.Phone, Value = "555" }
                }
            };

            _personRepositoryMock.Setup(r => r.GetByIdAsync(personId)).ReturnsAsync(person);

            var result = await _personService.DeleteContactInfoAsync(personId, contactInfoId);

            Assert.NotNull(result);
            Assert.DoesNotContain(result.ContactInfos, c => c.Id == contactInfoId);

            _personRepositoryMock.Verify(r => r.UpdateAsync(It.Is<Person>(p => p.Id == personId && !p.ContactInfos.Any(c => c.Id == contactInfoId))), Times.Once);
        }

        [Fact]
        public async Task DeleteContactInfoAsync_ShouldReturnNull_WhenPersonNotFound()
        {
            var personId = Guid.NewGuid();
            _personRepositoryMock.Setup(r => r.GetByIdAsync(personId)).ReturnsAsync((Person)null);

            var result = await _personService.DeleteContactInfoAsync(personId, Guid.NewGuid());

            Assert.Null(result);
        }

        [Fact]
        public async Task DeleteContactInfoAsync_ShouldReturnNull_WhenContactInfoNotFound()
        {
            var personId = Guid.NewGuid();
            var person = new Person { Id = personId, ContactInfos = new List<ContactInfo>() };
            _personRepositoryMock.Setup(r => r.GetByIdAsync(personId)).ReturnsAsync(person);

            var result = await _personService.DeleteContactInfoAsync(personId, Guid.NewGuid());

            Assert.Null(result);
        }
    }
}
