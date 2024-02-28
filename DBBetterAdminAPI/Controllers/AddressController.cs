using BetterAdminDbAPI.DTO;
using BetterAdminDbAPI.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace BetterAdminDbAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AddressController : ControllerBase
    {
        private readonly BetterAdminContext dbContext;

        public AddressController(BetterAdminContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet("GetAddresses")]
        public async Task<ActionResult<List<AddressDTO>>> Get()
        {
            var List = await dbContext.Addresses.Select(
                s => new AddressDTO
                {
                    Id = s.Id,
                    City = s.City,
                    Road = s.Road,
                    PostalCode = s.PostalCode
                }
            ).ToListAsync();

            if (List.Count < 0)
            {
                return NotFound();
            }
            else
            {
                return List;
            }
        }

        [HttpGet("GetAddressById")]
        public async Task<ActionResult<AddressDTO>> GetAddressById([FromBody] int id)
        {
            AddressDTO? Address = await dbContext.Addresses.Select(
                    s => new AddressDTO
                    {
                        Id = s.Id,
                        City = s.City,
                        Road = s.Road,
                        PostalCode = s.PostalCode
                    })
                .FirstOrDefaultAsync(s => s.Id == id);

            if (Address == null)
            {
                return NotFound();
            }
            else
            {
                return Address;
            }
        }

        [HttpPost("InsertAddress")]
        public async Task<HttpStatusCode> InsertAddress([FromBody] AddressDTO address)
        {
            var entity = new Address()
            {
                City = address.City,
                Road = address.Road,
                PostalCode = address.PostalCode
            };

            dbContext.Addresses.Add(entity);
            await dbContext.SaveChangesAsync();

            return HttpStatusCode.Created;
        }

        [HttpPut("UpdateAddress")]
        public async Task<HttpStatusCode> UpdateAddress([FromBody] AddressDTO address)
        {
            Address? entity = await dbContext.Addresses.FirstOrDefaultAsync(s => s.Id == address.Id);
            if(entity == null)
            {
                NotFound();
            }

            entity.City = address.City;
            entity.Road = address.Road;
            entity.PostalCode = address.PostalCode;

            await dbContext.SaveChangesAsync();
            return HttpStatusCode.OK;
        }

        [HttpDelete("DeleteAddress/{Id}")]
        public async Task<HttpStatusCode> DeleteAddress([FromBody] int id)
        {
            var entity = new Address()
            {
                Id = Id
            };
            dbContext.Addresses.Attach(entity);
            dbContext.Addresses.Remove(entity);
            await dbContext.SaveChangesAsync();
            return HttpStatusCode.OK;
        }
    }
}
