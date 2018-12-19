using System.Threading.Tasks;
using EPWebAPI.Interfaces;
using EPWebAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace EPWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestPaymentController : ControllerBase
    {
        private readonly IRequestPaymentRepository _requestPaymentRepo;

        public RequestPaymentController(IRequestPaymentRepository requestPaymentRepo)
        {
            _requestPaymentRepo = requestPaymentRepo;
        }

        [HttpGet("{id}",Name="GetRequestPayment")]
        public async Task<RequestPayment> GetById([FromRoute] int id)
        {
            return await _requestPaymentRepo.GetById(id);
        }

        [HttpPost]
        public IActionResult Create([FromBody]RequestPayment requestPayment){

            Task<int> RequestPaymentId = _requestPaymentRepo.Create(requestPayment);
            return CreatedAtRoute("GetRequestPayment",new {id = RequestPaymentId},requestPayment);
        }

    }
}