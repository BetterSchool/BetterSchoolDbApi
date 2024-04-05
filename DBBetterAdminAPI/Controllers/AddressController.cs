using Microsoft.AspNetCore.Mvc;
using System.Net;
using BetterAdminDbAPI.Model;

namespace BetterAdminDbAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AddressController : ControllerBase
    {
        [HttpGet("GetAddresses")]
        public HttpStatusCode Get()
        {
            //List<Address> addresses = AddressRepo.GetAll();
            if (addresses.Count < 0){
                return NotFound();
            }
            else{
                return addresses;
            }
        }

        [HttpGet("GetAddressById")]
        public Address GetAddressById([FromBody] int id)
        {
            //Address? address = AddressRepo.GetById(id);
            if (address == null){
                return NotFound();
            }
        
        return address;
        }

        [HttpPost("InsertAddress")]
        public HttpStatusCode InsertAddress([FromBody] Address address)
        {
            //var result = AddressRepo.Create(address);
            //if (result == false){
            //      return NotFound();
            //}
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
            
            //var result = AddressRepo.UpdateAddress(entity);
            //if (result == false){
            //      return NotFound();
            //}
            return HttpStatusCode.OK;
        }

        [HttpDelete("DeleteAddress/{Id}")]
        public HttpStatusCode DeleteAddress([FromBody] int id)
        {
            //var result = AddressRepo.Delete(id);
            //if (result == false){
            //      return NotFound();
            //}
            return HttpStatusCode.OK;
        }
    }
}
