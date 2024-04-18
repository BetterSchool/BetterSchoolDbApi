using BetterAdminDbAPI.Model;
using BetterAdminDbAPI.Repository;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using System.Net;
using System.Web.Http;
using FromBodyAttribute = Microsoft.AspNetCore.Mvc.FromBodyAttribute;
using HttpGetAttribute = Microsoft.AspNetCore.Mvc.HttpGetAttribute;
using HttpPostAttribute = Microsoft.AspNetCore.Mvc.HttpPostAttribute;
using HttpPutAttribute = Microsoft.AspNetCore.Mvc.HttpPutAttribute;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace BetterAdminDbAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GuardianController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly MySqlConnection _con;
        GuardianRepository _repo;

        public GuardianController(IConfiguration configuration)
        {
            _configuration = configuration;
            _con = new MySqlConnection(_configuration.GetConnectionString("MyDBConnection"));
            _repo = new GuardianRepository(_con);
        }
        //GET: api/<GuardianController>
        [HttpGet("GetGuardians")]
        public List<Guardian> Get()
        {
            List<Guardian> guardians = _repo.GetAll();
            if (guardians.Count < 0)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            else{
                return guardians;
            }
        }

        //GET api/<GuardianController>/5
        [HttpGet("GetGuardianByEmail")]
        public Guardian GetGuardianByEmail([FromBody] String email)
        {
            Guardian? guardian = _repo.Get(email);

            if (guardian == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);
            
            return guardian;
        }

        //POST api/<GuardianController>
        [HttpPost("InsertGuardian")]
        public HttpStatusCode InsertGuardian([FromBody] Guardian guardian)
        {
            
            Guardian? result = _repo.Create(guardian);
            if (result == null){
                  return HttpStatusCode.Conflict;
            }
            return HttpStatusCode.Created;
        }

        //PUT api/<GuardianController>/5
        [HttpPut("UpdateGuardian")]
        public HttpStatusCode UpdateGuardian([FromBody] Guardian guardian)
        {
            Guardian? entity = _repo.Get(guardian.Email);
            if (entity == null)
            {
                NotFound();
            }
            //Personal identification
            entity.Email = guardian.Email;
            entity.FirstName = guardian.FirstName;
            entity.LastName = guardian.LastName;
            entity.PhoneNo = entity.PhoneNo;

            //Address
            entity.Road = guardian.Road;
            entity.PostalCode = guardian.PostalCode;
            entity.City = guardian.City;

            //Password
            entity.Salt = guardian.Salt;
            entity.HashedSaltedPassword = guardian.HashedSaltedPassword;

            var result = _repo.Update(entity);
            if (result == false){
                  return HttpStatusCode.Conflict;
            }
            else{
                return HttpStatusCode.OK;
            }
        }

        //DELETE api/<GuardianController>/5
        [HttpGet("DeleteGuardianById")]
        public HttpStatusCode DeleteGuardian(Guardian guardian)
        {
            var result = _repo.Delete(guardian);
            if (result == false){
                  return HttpStatusCode.NotFound;
            }
            return HttpStatusCode.OK;
        }
    }
}
