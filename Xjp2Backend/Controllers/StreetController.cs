using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Xjp2Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StreetController : ControllerBase
    {
        // GET: api/<StreetController>
        [HttpGet("[action]")]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<StreetController>/5
        [HttpGet("[action]/{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<StreetController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<StreetController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<StreetController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
