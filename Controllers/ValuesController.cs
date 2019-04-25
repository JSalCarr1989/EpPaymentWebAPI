using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EPPCIDAL.Interfaces;

namespace EPWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {

        private readonly IEnvironmentSettingsService _environmentSettingsService;
        private readonly IDbConnectionService _dbConnectionService;

        public ValuesController(
            IEnvironmentSettingsService environmentSettingsService,
            IDbConnectionService dbConnectionService
            )
        {
            _environmentSettingsService = environmentSettingsService;
            _dbConnectionService = dbConnectionService;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { $"Entorno de Ejecución: {_environmentSettingsService.GetAspNetCoreEnvironment()}", $"Connecion DB: { _dbConnectionService.ValidateDbConnection()}" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
