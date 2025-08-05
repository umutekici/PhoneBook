using ContactMicroservice.Application.Interfaces;
using ContactMicroservice.Domain.Entities;
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
    }
}
