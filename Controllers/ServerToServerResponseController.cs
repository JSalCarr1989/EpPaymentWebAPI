using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace EPWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServerToServerResponseController : ControllerBase
    {
        
        
        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {

        }
    }
}
