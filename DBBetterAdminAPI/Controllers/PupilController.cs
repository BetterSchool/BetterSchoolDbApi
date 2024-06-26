﻿using Microsoft.AspNetCore.Mvc;
using BetterAdminDbAPI.Model;
using BetterAdminDbAPI.Repository;
using System.Net;
using MySqlConnector;
using System.Web.Http;
using HttpGetAttribute = Microsoft.AspNetCore.Mvc.HttpGetAttribute;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;
using HttpPostAttribute = Microsoft.AspNetCore.Mvc.HttpPostAttribute;
using FromBodyAttribute = Microsoft.AspNetCore.Mvc.FromBodyAttribute;
using HttpPutAttribute = Microsoft.AspNetCore.Mvc.HttpPutAttribute;
using HttpDeleteAttribute = Microsoft.AspNetCore.Mvc.HttpDeleteAttribute;
using Microsoft.AspNetCore.Server.IIS.Core;

namespace BetterAdminDbAPI.Controllers
{


    [ApiController]
    [Route("api/[controller]")]
    public class PupilController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        PupilRepository repo;

        public PupilController(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("MyDBConnection");
            repo = new PupilRepository(_connectionString);
        }

        // GET: api/<PupilController>
        [HttpGet]
        public List<Pupil> Get()
        {
            List<Pupil> pupils = repo.GetAll();
            if (pupils.Count < 0){
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
                return pupils;
        }

        // GET api/<PupilController>/5
        [HttpGet("GetByEmail")]
        public Pupil GetByEmail([FromUri]string email)
        {
            Pupil? pupil = repo.Get(email);
            if (pupil == null){
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        
        return pupil;
        }
    

        // POST api/<PupilController>
        [HttpPost]
        public Pupil Post([FromBody]Pupil pupil)
        {
            //The order of the executed code is important, as the code is requesting the use of the database, which has it's own constraints and error handling.

            //Create guardian if:
            if (pupil == null){
                throw new HttpResponseException(HttpStatusCode.Conflict); 
            }
                var result = repo.Create(pupil);

                if (result == null){
                    throw new HttpResponseException(HttpStatusCode.Conflict);
                }

            return result;
        }

        // PUT api/<PupilController>/5
        [HttpPut]
        public Pupil UpdatePupil([FromBody] Pupil pupilChanges)
        {
            Pupil? result = repo.Get(pupilChanges.Email);
            if (result == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            Pupil? updateResult = repo.Update(pupilChanges);

            if (updateResult == null){
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return updateResult;
        }

        // DELETE api/<PupilController>/5
        [HttpDelete]
        public HttpStatusCode DeletePupil([FromBody]Pupil pupil)
        {
            var result = repo.Delete(pupil);

            if (result == false){
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return HttpStatusCode.OK;
        }
    }
}
