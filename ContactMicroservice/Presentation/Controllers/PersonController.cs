using ContactMicroservice.Application.DTOs;
using ContactMicroservice.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ContactMicroservice.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly IPersonService _personService; 

        public PersonController(IPersonService personService)
        {
            _personService = personService;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePerson([FromBody] PersonDto personDto)
        {
            if (personDto == null)
                return BadRequest("Person data is required.");

            var createdPerson = await _personService.CreatePersonAsync(personDto);
            return CreatedAtAction(nameof(GetPersonById), new { id = createdPerson.Id }, createdPerson);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPersonById(Guid id)
        {
            var person = await _personService.GetPersonByIdAsync(id);
            if (person == null)
                return NotFound();

            return Ok(person);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPersons()
        {
            var persons = await _personService.GetAllPersonsAsync();
            return Ok(persons);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePerson(Guid id)
        {
            await _personService.DeletePersonAsync(id);
            return NoContent();
        }

        [HttpPost("{personId}/contactinfos")]
        public async Task<IActionResult> AddContactInfo(Guid personId, [FromBody] ContactInfoDto contactInfoDto)
        {
            var updatedPerson = await _personService.AddContactInfoAsync(personId, contactInfoDto);
            if (updatedPerson == null)
                return NotFound("Person not found.");

            return Ok(updatedPerson);
        }

        [HttpDelete("{personId}/contactinfos/{contactInfoId}")]
        public async Task<IActionResult> DeleteContactInfo(Guid personId, Guid contactInfoId)
        {
            var updatedPerson = await _personService.DeleteContactInfoAsync(personId, contactInfoId);
            if (updatedPerson == null)
                return NotFound("Person or ContactInfo not found.");

            return Ok(updatedPerson);
        }
    }
}
