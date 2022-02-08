using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ComicsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComicsScoreController : ControllerBase
    {
        // GET: api/ComicsScore
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/ComicsScore/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/ComicsScore
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/ComicsScore/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ComicsScore/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
