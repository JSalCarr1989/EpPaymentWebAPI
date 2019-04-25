using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EPPCIDAL.Interfaces;
using EPPCIDAL.Models;
using System;

namespace EPWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestPaymentController : ControllerBase
    {
        
        private readonly IRequestPaymentService _requestPaymentService;

        public RequestPaymentController(
            
            IRequestPaymentService requestPaymentService
            )
        {
            
            _requestPaymentService = requestPaymentService;
        }

        [HttpGet("{id}",Name="GetRequestPayment")]
        public async Task<RequestPayment> GetById([FromRoute] int id)
        {
            RequestPayment requestPayment = null;

            try
            {
                requestPayment =  await _requestPaymentService.GetByIdAsync(id);
            }
            catch(Exception ex)
            {

            }
            

            return requestPayment;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]RequestPayment requestPayment){

            int RequestPaymentId =  await _requestPaymentService.CreateAsync(requestPayment);
            return CreatedAtRoute("GetRequestPayment",new {id = RequestPaymentId},requestPayment);
        }

    }
}