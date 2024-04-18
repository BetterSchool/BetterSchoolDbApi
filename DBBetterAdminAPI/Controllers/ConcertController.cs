using System.Net;
using System.Web.Http;
using BetterAdminDbAPI.Model;
using BetterAdminDbAPI.Repository;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using FromBodyAttribute = Microsoft.AspNetCore.Mvc.FromBodyAttribute;
using HttpGetAttribute = Microsoft.AspNetCore.Mvc.HttpGetAttribute;
using HttpPostAttribute = Microsoft.AspNetCore.Mvc.HttpPostAttribute;
using HttpPutAttribute = Microsoft.AspNetCore.Mvc.HttpPutAttribute;

namespace BetterAdminDbAPI.Controllers;

public class ConcertController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly MySqlConnection _con;
    ConcertRepository _repo;
    public ConcertController(IConfiguration configuration)
    {
            _configuration = configuration;
            
            _con = new MySqlConnection(_configuration.GetConnectionString("MyDBConnection"));
            
            _repo = new ConcertRepository(_con);
    }
    [HttpGet("GetConcert")]
    public List<Concert> Get()
    {
        List<Concert> concert = _repo.GetAll();
        if (concert.Count < 0)
        {
            throw new HttpResponseException(HttpStatusCode.NotFound);
        }
         else{
            return concert;
        }
    }

    [HttpGet("GetConcertByEmail")]
    public Concert GetConcertByEmail([FromBody] int id)
    {
        Concert? concert = _repo.Get(id);

        if (concert == null)
            throw new HttpResponseException(HttpStatusCode.NotFound);
            
        return concert;
    }

    [HttpPost("InsertConcert")]
    public HttpStatusCode InsertConcert([FromBody] Concert concert)
    {
            
        Concert? result = _repo.Create(concert);
        if (result == null){
                return HttpStatusCode.Conflict;
        }
        return HttpStatusCode.Created;
    }

    [HttpPut("UpdateConcert")]
    public HttpStatusCode UpdateGuardian([FromBody] Concert concert)
    {
        Concert? entity = _repo.Get(concert.ConcertId);
        if (entity == null)
        {
            NotFound();
        }
            
        entity.ConcertName = concert.ConcertName;
        entity.ConcertLocation = concert.ConcertLocation;
        entity.StartTime = concert.StartTime;
        entity.EndTime = entity.EndTime;

        var result = _repo.Update(entity);
        if (result == false){
                return HttpStatusCode.Conflict;
        }
        else{
            return HttpStatusCode.OK;
        }
    }

    [HttpGet("DeleteConcert")]
    public HttpStatusCode DeleteConcert(Concert concert)
    {
        bool result = _repo.Delete(concert);
        if (result == false){
                return HttpStatusCode.NotFound;
        }
        return HttpStatusCode.OK;
    }
}