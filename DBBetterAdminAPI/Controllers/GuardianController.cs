using Microsoft.AspNetCore.Mvc;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

//namespace BetterAdminDbAPI.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class GuardianController : ControllerBase
//    {
//        // GET: api/<GuardianController>
//        [HttpGet("GetGuardians")]
//        public Guardian Get()
//        {
//            ////List<Guardian> guardians = GuardianRepo.GetAll();
//            if (guardians.Count < 0){
//                return NotFound();
//            }
//            else{
//                return guardians;
//            }
//        }

//        // GET api/<GuardianController>/5
//        [HttpGet("GetGuardianByEmail")]
//        public Guardian GetGuardianByEmail([FromBody] String email)
//        {
//            Guardian? guardian = //GuardianRepo.GetByEmail(email);

//            if (guardian == null)
//                return NotFound();
            
//            return guardian;
//        }

//        // POST api/<GuardianController>
//        [HttpPost("InsertGuardian")]
//        public HttpStatusCode InsertGuardian([FromBody] Guardian guardian)
//        {
            
//            //var result = GuardianRepo.Add(guardian);
//            //if (result == false){
//            //      return HttpStatusCode.Conflict;
//            //}
//            return HttpStatusCode.Created;
//        }

//        // PUT api/<GuardianController>/5
//        [HttpPut("UpdateGuardian")]
//        public HttpStatusCode UpdateGuardian([FromBody] Guardian guardian)
//        {
//            Guardian? entity = //GuardianRepo.GetByEmail(guardian.Email);
//            if (entity == null)
//            {
//                NotFound();
//            }

//            entity.Id = guardian.Id;
//            entity.WorkPhoneNo = guardian.WorkPhoneNo;
//            entity.AddressId = guardian.AddressId;
//            entity.PersonId = guardian.PersonId;
//            entity.Email = guardian.Email;

//            //var result = GuardianRepo.Update(entity);
//            //if (result == false){
//            //      return HttpStatusCode.Conflict();
//            //}
//            else{
//                return HttpStatusCode.OK;
//            }
//        }

//        // DELETE api/<GuardianController>/5
//        [HttpGet("DeleteGuardianById")]
//        public HttpStatusCode DeleteGuardian(int id)
//        {
//            //var result = GuardianRepo.Delete(id);
//            //if (result == false){
//            //      return NotFound();
//            //}
//            return HttpStatusCode.OK;
//        }
//    }
//}
