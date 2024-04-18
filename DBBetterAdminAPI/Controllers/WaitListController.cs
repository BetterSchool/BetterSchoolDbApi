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

public class WaitListController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly MySqlConnection _con;
    WaitListRepository _repo;
    public WaitListController(IConfiguration configuration)
    {
            _configuration = configuration;
            
            _con = new MySqlConnection(_configuration.GetConnectionString("MyDBConnection"));
            
            _repo = new WaitListRepository(_con);
    }
    [HttpGet("GetWaitList")]
    public List<WaitList> Get()
    {
        List<WaitList> waitList = _repo.GetAll();
        if (waitList.Count < 0)
        {
            throw new HttpResponseException(HttpStatusCode.NotFound);
        }
         else{
            return waitList;
        }
    }

    [HttpGet("GetWaitListById")]
    public WaitList GetWaitListById([FromBody] int id)
    {
        WaitList? waitList = _repo.Get(id);

        if (waitList == null)
            throw new HttpResponseException(HttpStatusCode.NotFound);
            
        return waitList;
    }

    [HttpPost("InsertWaitList")]
    public HttpStatusCode InsertWaitList([FromBody] WaitList waitList)
    {
        //
        WaitList? results =_repo.Get(waitList);
        if (results != null)
        {
            return HttpStatusCode.Forbidden;
        }
            
        WaitList? result = _repo.Create(waitList);
        if (result == null){
                return HttpStatusCode.Conflict;
        }
        return HttpStatusCode.Created;
    }

    [HttpPut("UpdateWaitList")]
    public HttpStatusCode UpdateGuardian([FromBody] WaitList waitList)
    {
        WaitList? entity = _repo.Get(waitList);
        if (entity == null)
        {
            //NotFound();
        }
            
        entity.CourseId = waitList.CourseId;
        entity.PupilId = waitList.PupilId;
        entity.TimeEnteredQueue = waitList.TimeEnteredQueue;

        var result = _repo.Update(entity);
        if (result == false){
                return HttpStatusCode.Conflict;
        }
        else{
            return HttpStatusCode.OK;
        }
    }

    [HttpGet("DeleteWaitList")]
    public HttpStatusCode DeleteWaitList(WaitList waitList)
    {
        bool result = _repo.Delete(waitList);
        if (result == false){
                return HttpStatusCode.NotFound;
        }
        return HttpStatusCode.OK;
    }
}