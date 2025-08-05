using ContactMicroservice.Application.DTOs;
using ContactMicroservice.Application.Interfaces;
using ContactMicroservice.Domain.Entities;
using ContactMicroservice.Domain.Enums;
using ContactMicroservice.Presentation.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace ContactMicroservice.Tests.Presentation
{
    public class PersonControllerTests
    {
        private readonly Mock<IPersonService> _mockPersonService;
        private readonly PersonController _controller;

        public PersonControllerTests()
        {
            _mockPersonService = new Mock<IPersonService>();
            _controller = new PersonController(_mockPersonService.Object);
        }

        [Fact]
        public async Task GetPersonById_ReturnsOk_WithPerson_WhenPersonExists()
        {
            var personId = Guid.NewGuid();
            var person = new Person
            {
                Id = personId,
                FirstName = "Umut",
                LastName = "Ekici",
                Company = "MyCompany"
            };

            _mockPersonService.Setup(s => s.GetPersonByIdAsync(personId))
                              .ReturnsAsync(person);

            var result = await _controller.GetPersonById(personId);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedPerson = Assert.IsType<Person>(okResult.Value);
            Assert.Equal(personId, returnedPerson.Id);
            Assert.Equal("Umut", returnedPerson.FirstName);
        }

        [Fact]
        public async Task GetPersonById_ReturnsNotFound_WhenPersonDoesNotExist()
        {
            var personId = Guid.NewGuid();

            _mockPersonService.Setup(s => s.GetPersonByIdAsync(personId))
                              .ReturnsAsync((Person)null);

            var result = await _controller.GetPersonById(personId);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task CreatePerson_ReturnsCreatedAtActionResult_WithCreatedPerson()
        {
            var personDto = new PersonDto
            {
                FirstName = "Umut",
                LastName = "Ekici",
                Company = "MyCompany"
            };

            var createdPerson = new Person
            {
                Id = Guid.NewGuid(),
                FirstName = personDto.FirstName,
                LastName = personDto.LastName,
                Company = personDto.Company
            };

            _mockPersonService.Setup(s => s.CreatePersonAsync(personDto))
                              .ReturnsAsync(createdPerson);

            var result = await _controller.CreatePerson(personDto);

            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnedPerson = Assert.IsType<Person>(createdAtActionResult.Value);
            Assert.Equal(createdPerson.Id, returnedPerson.Id);
            Assert.Equal("Umut", returnedPerson.FirstName);
        }

        [Fact]
        public async Task CreatePerson_ReturnsBadRequest_WhenPersonDtoIsNull()
        {
            PersonDto personDto = null;

            var result = await _controller.CreatePerson(personDto);

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task GetAllPersons_ReturnsOk_WithPersonList()
        {
            var persons = new List<Person>
            {
                new Person { Id = Guid.NewGuid(), FirstName = "Umut", LastName = "Ekici", Company = "CompanyA" },
                new Person { Id = Guid.NewGuid(), FirstName = "Ali", LastName = "Veli", Company = "CompanyB" }
            };

            _mockPersonService.Setup(s => s.GetAllPersonsAsync())
                              .ReturnsAsync(persons);

            var result = await _controller.GetAllPersons();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedList = Assert.IsType<List<Person>>(okResult.Value);
            Assert.Equal(2, returnedList.Count);
        }

        [Fact]
        public async Task GetAllPersons_ReturnsOk_WithEmptyList()
        {
            var persons = new List<Person>();

            _mockPersonService.Setup(s => s.GetAllPersonsAsync())
                              .ReturnsAsync(persons);

            var result = await _controller.GetAllPersons();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedList = Assert.IsType<List<Person>>(okResult.Value);
            Assert.Empty(returnedList);
        }

        [Fact]
        public async Task DeletePerson_ReturnsNoContent()
        {
            var personId = Guid.NewGuid();

            _mockPersonService.Setup(s => s.DeletePersonAsync(personId))
                              .Returns(Task.CompletedTask);

            var result = await _controller.DeletePerson(personId);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task AddContactInfo_ReturnsOk_WhenPersonExists()
        {
            var personId = Guid.NewGuid();
            var contactInfoDto = new ContactInfoDto
            {
                Type = ContactType.Email,
                Value = "umut@example.com"
            };

            var updatedPerson = new Person
            {
                Id = personId,
                FirstName = "Umut",
                LastName = "Ekici",
                Company = "MyCompany",
                ContactInfos = new List<ContactInfo>
                {
                    new ContactInfo { Id = Guid.NewGuid(), Type = ContactType.Email, Value = "umut@example.com" }
                }
            };

            _mockPersonService.Setup(s => s.AddContactInfoAsync(personId, contactInfoDto))
                              .ReturnsAsync(updatedPerson);

            var result = await _controller.AddContactInfo(personId, contactInfoDto);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedPerson = Assert.IsType<Person>(okResult.Value);
            Assert.Equal(personId, returnedPerson.Id);
            Assert.Single(returnedPerson.ContactInfos);
        }

        [Fact]
        public async Task AddContactInfo_ReturnsNotFound_WhenPersonDoesNotExist()
        {
            var personId = Guid.NewGuid();
            var contactInfoDto = new ContactInfoDto
            {
                Type = ContactType.Phone,
                Value = "1234567890"
            };

            _mockPersonService.Setup(s => s.AddContactInfoAsync(personId, contactInfoDto))
                              .ReturnsAsync((Person)null);

            var result = await _controller.AddContactInfo(personId, contactInfoDto);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Person not found.", ((dynamic)notFoundResult.Value).Message);
        }

    }
}
