using BetterAdminDbAPI.DTO;
using BetterAdminDbAPI.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BetterAdminDbAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GuardianController : ControllerBase
    {
        private readonly BetterAdminContext dbContext;

        public GuardianController(BetterAdminContext dbContext)
        {
            this.dbContext = dbContext;
        }

        // GET: api/<GuardianController>
        [HttpGet("GetGuardians")]
        public async Task<ActionResult<List<GuardianDTO>>> Get()
        {
            var List = await dbContext.Guardians.Select(
                guardian => new GuardianDTO
                {
                    Id = guardian.Id,
                    WorkPhoneNo = guardian.WorkPhoneNo,
                    AddressId = guardian.AddressId,
                    PersonId = guardian.PersonId,
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

        // GET api/<GuardianController>/5
        [HttpGet("GetGuardianById")]
        public async Task<ActionResult<GuardianDTO>> GetGuardianById([FromBody] int id)
        {
            Guardian? guardian = await dbContext.Guardians.SingleOrDefaultAsync(e => e.Id == id);

            if (guardian == null)
                return NotFound();

            GuardianDTO guardianDTO = new GuardianDTO
            {
                Id = guardian.Id,
                WorkPhoneNo = guardian.WorkPhoneNo,
                AddressId = guardian.AddressId,
                PersonId = guardian.PersonId,
            };

            return guardianDTO;
        }

        // POST api/<GuardianController>
        [HttpPost]
        public async Task<HttpStatusCode> Post([FromBody] Guardian guardian)
        {
            var entity = new Guardian()
            {
                Id = guardian.Id,
                WorkPhoneNo = guardian.WorkPhoneNo,
                PersonId = guardian.PersonId,
                AddressId = guardian.AddressId
            };

            dbContext.Guardians.Add(entity);
            await dbContext.SaveChangesAsync();

            return HttpStatusCode.Created;
        }

        // PUT api/<GuardianController>/5
        [HttpPut("UpdateGuardian")]
        public async Task<HttpStatusCode> UpdateGuardian([FromBody] GuardianDTO guardian)
        {
            Guardian? entity = await dbContext.Guardians.FirstOrDefaultAsync(s => s.Id == guardian.Id);
            if (entity == null)
            {
                NotFound();
            }

            entity.Id = guardian.Id;
            entity.WorkPhoneNo = guardian.WorkPhoneNo;
            entity.AddressId = guardian.AddressId;
            entity.PersonId = guardian.PersonId;

            await dbContext.SaveChangesAsync();
            return HttpStatusCode.OK;
        }

        // DELETE api/<GuardianController>/5
        [HttpGet("DeleteGuardianById")]
        public async Task<HttpStatusCode> DeleteGuardian(int id)
        {
            var entity = new Guardian()
            {
                Id = id
            };
            dbContext.Guardians.Attach(entity);
            dbContext.Guardians.Remove(entity);
            await dbContext.SaveChangesAsync();
            return HttpStatusCode.OK;
        }
    }
}
