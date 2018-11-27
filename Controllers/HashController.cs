using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EPWebAPI.Interfaces;
using EPWebAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace EPWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HashController : ControllerBase
    {

        private readonly IHashRepository _hashRepo;

        public HashController(IHashRepository hashRepo)
        {
           _hashRepo = hashRepo;
        }

        [HttpPost]
        public Hash CreateRequestHash([FromBody] HashDTO hash)
        {
            return _hashRepo.CreateRequestHash(hash);
        }

    }

}