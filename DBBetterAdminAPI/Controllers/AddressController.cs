using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BetterAdminDbAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AddressController : ControllerBase
    {

        [HttpGet("GetAddresses")]
        public List<Address> Get()
        {
            
        }

        [HttpGet("GetAddressById")]
        public Address GetAddressById([FromBody] int id)
        {

        }

        [HttpPost("InsertAddress")]
        public HttpStatusCode InsertAddress([FromBody] Address address)
        {

            return HttpStatusCode.Created;
        }

        [HttpPut("UpdateAddress")]
        public HttpStatusCode UpdateAddress([FromBody] Address address)
        {
            Address? entity = //Repo.Get(Address);
            if(entity == null)
            {
                NotFound();
            }

            entity.City = address.City;
            entity.Road = address.Road;
            entity.PostalCode = address.PostalCode;
            
            //AddressRepo.UpdateAddress(entity);
            return HttpStatusCode.OK;
        }

        [HttpDelete("DeleteAddress/{Id}")]
        public HttpStatusCode DeleteAddress([FromBody] int id)
        {
            //AddressRepo.Delete(id);
            return HttpStatusCode.OK;
        }
    }
}
