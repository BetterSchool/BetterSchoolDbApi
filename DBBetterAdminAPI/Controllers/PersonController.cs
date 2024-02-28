using BetterAdminDbAPI.DTO;
using BetterAdminDbAPI.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Utilities.Net;
using System.Net;
using System.Threading.Channels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BetterAdminDbAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly BetterAdminContext dbContext;

        public PersonController(BetterAdminContext Context)
        {
            this.dbContext = Context;
        }
        // GET: api/<PersonController>
        [HttpGet("GetPeople")]
        public async Task<ActionResult<List<PersonDTO>>> Get()
        {
            List<PersonDTO> List = await dbContext.People.Select(
                s => new PersonDTO
                {
                    Id = s.Id,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    PhoneNumber = s.PhoneNo,
                    Email = s.Email
                }).ToListAsync();

            if (List.Count < 0)
            {
                return NotFound();
            }
            else
            {
                return List;
            }
        }

        // GET api/<PersonController>/5
        [HttpGet("GetPersonById")]
        public async Task<ActionResult<PersonDTO>> GetPersonById([FromBody] int id)
        {
            Person? person = await dbContext.People.FirstOrDefaultAsync(s => s.Id == id);
            if (person == null)
                return NotFound();

            PersonDTO personDTO = new PersonDTO 
            {
                Id = person.Id,
                FirstName = person.FirstName,
                LastName = person.LastName,
                PhoneNumber = person.PhoneNo,
                Email = person.Email
            };
                return personDTO;
        }

        // POST api/<PersonController>
        [HttpPost("InsertPerson")]
        public async Task<HttpStatusCode> Post([FromBody] PersonDTO personDTO)
        {
            Person person = new Person()
            {
                FirstName = personDTO.FirstName,
                LastName = personDTO.LastName,
                PhoneNo = personDTO.PhoneNumber,
                Email = personDTO.Email
            };

            dbContext.People.Add(person);
            await dbContext.SaveChangesAsync();
            return HttpStatusCode.OK;
        }

        // PUT api/<PersonController>/5
        [HttpPut("UpdatePersonById")]
        public async Task<HttpStatusCode> UpdatePerson(int id, [FromBody] PersonDTO changes)
        {
            Person? entity = await dbContext.People.FirstOrDefaultAsync(s => s.Id == changes.Id);

            if (entity == null)
                return HttpStatusCode.NotFound;
            

            entity.FirstName = changes.FirstName;
            entity.LastName = changes.LastName;
            entity.PhoneNo = changes.PhoneNumber;
            entity.Email = changes.Email;

            await dbContext.SaveChangesAsync();
            return HttpStatusCode.OK;
        }

        // DELETE api/<PersonController>/5
        [HttpDelete("DeletePersonById")]
        public async Task<HttpStatusCode> DeletePersonById(int id)
        {
            Person? entity = await dbContext.People.FirstOrDefaultAsync(s => s.Id == id);
            if (entity == null)
                return HttpStatusCode.NotFound;

            dbContext.People.Attach(entity);
            dbContext.People.Remove(entity);
            await dbContext.SaveChangesAsync();
            return HttpStatusCode.OK;
        }
    }
}
