using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using log4net.DateFormatter;
using membermicroservice.Models;
using membermicroservice.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace membermicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class memberController : ControllerBase
    {
        Imemberclaimrepo repo;
        readonly log4net.ILog _log4net;
        Uri baseAddress = new Uri("http://20.193.144.70/api"); //claim  = https://localhost:44393/api
        HttpClient client;
        public memberController(Imemberclaimrepo _rep)
        {
            client = new HttpClient();
            client.BaseAddress = baseAddress;
            _log4net = log4net.LogManager.GetLogger(typeof(memberController));
            repo = _rep;

        }


        //check the http verbs
        
        [HttpGet("{id}")]
       
        public List<memberclaim> Get(int id)//first fetch all the claims and then filter it
        {

            _log4net.Info("MemberController Get View Claim Action Method is called!");


            List<memberclaim> ls = new List<memberclaim>();


            HttpResponseMessage response = client.GetAsync(client.BaseAddress + "/claim").Result;//calls getall method in claim controller

            if (response.IsSuccessStatusCode)
            {

                string data = response.Content.ReadAsStringAsync().Result;

                ls = JsonConvert.DeserializeObject<List<memberclaim>>(data);

            }
            //filtering process
            
            List<memberclaim> ml = new List<memberclaim>();

            foreach(var item in ls)
            {
                if(item.memberid== id)
                {
                    ml.Add(item);
                }
            }

            return ml;
        }

     /// <summary>
     /// It is used to send the memberclaim object received from the memberportal to the claim microservice and stores it in the memberclaim static list in claim microservice
     /// </summary>
     /// <param name="obj"></param>
     /// <returns></returns>
  
       
        [HttpPost]
        public IActionResult Post([FromBody] memberclaim obj)//for submitting claims
        {
            _log4net.Info("MemberController Submit Claim Action Method is called!");
           
            if (repo.submitClaim(obj) == "success")
                return Ok("success");
            return BadRequest("failure");
        }
      
     /// <summary>
     /// This function returns the claim status for a particular claim by invoking the claim microservice and updates the status in memberclaim static list in claim microservice
     /// </summary>
     /// <param name="id"></param>
     /// <param name="obj"></param>
     /// <returns></returns>
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] memberclaim obj)//for viewing claim status
        {
            _log4net.Info("MemberController View Claim Status Action Method is called!");
            memberclaim ob = repo.viewClaimStatus(id, obj);
            if (ob == null)
                return BadRequest(ob);
            else
                return Ok(ob);
        }

    }
}
