using Microsoft.AspNetCore.Mvc;
using EPPCIDAL.Interfaces;
using EPPCIDAL.Models;
using EPPCIDAL.DTO;
using EPPCIDAL.Services;


namespace EPWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HashController : ControllerBase
    {

        //private readonly IHashRepository _hashRepo;
        private readonly IHashService _hashService;

        public HashController(IHashService hashService)
        {
           _hashService = hashService;
        }

        [HttpPost]
        public Hash CreateRequestHash([FromBody] HashDTO hash)
        {
            return _hashService.CreateRequestHash(hash);
        }

    }

}