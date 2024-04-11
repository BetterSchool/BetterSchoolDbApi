using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Reflection;

namespace BetterAdminDbAPI.Controllers
{
    

    [ApiController]
    [Route("api/[controller]")]
    public class PupilController : ControllerBase
    {
        // GET: api/<PupilController>
        [HttpGet("GetPupils")]
        public HttpStatusCode Get()
        {
            //List<Pupil> pupils = AddressRepo.GetAll();
            if (pupils.Count < 0){
                return NotFound();
            }
                return pupils;
        }

        // GET api/<PupilController>/5
        [HttpGet("GetPupilByEmail")]
        public Address GetPupilByEmail([FromBody] String email)
        {
            //Pupil? pupil = PupilRepo.GetByEmail(email);
            if (pupil == null){
            return NotFound();
            }
        
        return pupil;
        }
    

        // POST api/<PupilController>
        [HttpPost]
        public HttpStatusCode Post([FromBody]Pupil pupil, Guardian? guardian)
        {
            //The order of the executed code is important, as the code is requesting the use of the database, which has it's own constraints and error handling.

            //Create guardian if:
            if (pupil == null){
                return HttpStatusCode.Conflict;
            }
            if (guardian != null){
                //var result = PupilRepo.CreatePupil(pupil, guardian);
                //if (result == false){
                //return HttpStatusCode.Conflict;
                //}
            }
            else{
                //var result = PupilRepo.CreatePupil(pupil);
                //if (result == false){
                //return HttpStatusCode.Conflict;
                //}
            }
            return HttpStatusCode.Created;
        }

        // PUT api/<PupilController>/5
        [HttpPut("UpdatePupil")]
        public async Task<HttpStatusCode> UpdatePupil([FromBody] String email, Pupil pupilChanges)
        {
            //var result = PupilRepo.Update(email, pupilChanges)
            //if (result == false){
            //return HttpStatusCode.Conflict;
            //}

            return HttpStatusCode.OK;
        }

        // DELETE api/<PupilController>/5
        [HttpGet("DeletePupilById")]
        public async Task<HttpStatusCode> Delete(int id)
        {
            //var result = PupilRepo.DeleteById(id)
            //if (result == false){
            //return HttpStatusCode.Conflict;
            //}
            return HttpStatusCode.OK;
        }
    }
}
