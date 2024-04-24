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

namespace BetterAdminDbAPI.Controllers
{


    [ApiController]
    [Route("api/[controller]")]
    public class PupilController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        PupilRepository _repo;

        public PupilController(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("MyDBConnection");
            _repo = new PupilRepository(_connectionString);
        }

        // GET: api/<PupilController>
        [HttpGet("GetPupils")]
        public List<Pupil> Get()
        {
            List<Pupil> pupils = _repo.GetAll();
            if (pupils.Count < 0){
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
                return pupils;
        }

        // GET api/<PupilController>/5
        [HttpGet("GetPupilByEmail")]
        public Pupil GetPupilByEmail([FromUri]string email)
        {
            Pupil? pupil = _repo.Get(email);
            if (pupil == null){
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        
        return pupil;
        }
    

        // POST api/<PupilController>
        [HttpPost]
        public Pupil Post([FromBody]Pupil pupil)
        {
            //Check for already existing Pupil
            Pupil? entry = _repo.Get(pupil.Email);

            if (entry != null)
            {
                throw new HttpResponseException(HttpStatusCode.Conflict); 
            }

            var result = _repo.Create(pupil);

            if (result == null)
            {
                throw new HttpResponseException(HttpStatusCode.Conflict);
            }

            return result;
        }

        // PUT api/<PupilController>/5
        [HttpPut("UpdatePupil")]
        public HttpStatusCode UpdatePupil([FromBody] Pupil pupilChanges)
        {
            var result = _repo.Update(pupilChanges);

            if (result == null){
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return HttpStatusCode.OK;
        }

        // DELETE api/<PupilController>/5
        [HttpPut("DeletePupilByEmail")]
        public HttpStatusCode DeletePupilById([FromUri]string email)
        {
            var result = _repo.Delete(email);

            if (result == false){
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return HttpStatusCode.OK;
        }
    }
}
