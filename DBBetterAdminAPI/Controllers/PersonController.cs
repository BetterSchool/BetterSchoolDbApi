using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Channels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BetterAdminDbAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        // GET: api/<PersonController>
        [HttpGet("GetPeople")]
        public Person Get()
        {
            ////List<Person> people = PersonRepo.GetAll();
            if (people.Count < 0){
                return NotFound();
            }
            return people;
        }

        // GET api/<PersonController>/5
        [HttpGet("GetPersonByEmail")]        
        public Person GetPersonByEmail([FromBody] String email)
        {
            Person? person = //PersonRepo.GetByEmail(email);

            if (person == null)
                return NotFound();
            
            return person;
        }

        // POST api/<PersonController>
        [HttpPost("InsertPerson")]
        public HttpStatusCode InsertPerson([FromBody] Person person)
        {
            
            //var result = PersonRepo.Add(person);
            //if (result == false){
            //      return HttpStatusCode.Conflict;
            //}
            return HttpStatusCode.Created;
        }

        // PUT api/<PersonController>/5
        [HttpPut("UpdatePerson")]
       public HttpStatusCode UpdatePerson([FromBody] Person person)
        {
            Person? entity = //PersonRepo.GetByEmail(person.Email);
            if (entity == null)
            {
                NotFound();
            }

            entity.Id = person.Id;
            entity.FirstName = person.FirstName;
            entity.LastName = person.LastName;
            entity.PhoneNo = person.PhoneNumber;
            entity.Email = person.Email;

            //var result = PersonRepo.Update(entity);
            //if (result == false){
            //      return HttpStatusCode.Conflict;
            //}
            return HttpStatusCode.OK;
        }

        // DELETE api/<PersonController>/5
        [HttpDelete("DeletePersonById")]
        public HttpStatusCode DeletePerson(int id)
        {
            //var result = PersonRepo.Delete(id);
            //if (result == false){
            //      return NotFound();
            //}
            return HttpStatusCode.OK;
        }
    }
}
