using Microsoft.AspNetCore.Mvc;
using System;
using EPPCIDAL.Models;
using EPPCIDAL.Interfaces;
using EPPCIDAL.DTO;
using EPPCIDAL.Constants;

namespace EPWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServerToServerResponseController : ControllerBase
    {

        private readonly IResponsePaymentService _responsePaymentService;
        private readonly IEndPaymentService _endPaymentService;
        



        public ServerToServerResponseController(
                            IResponsePaymentService responsePaymentService,
                            IEndPaymentService endPaymentService
            )
        {
            _responsePaymentService = responsePaymentService;
            _endPaymentService = endPaymentService;
            
        }
        
        // POST api/values
        [HttpPost]
        public async void Post([FromForm] MultiPagosResponsePaymentDTO multipagosResponse)
        {
            try
            {
                int responsePaymentId = await _responsePaymentService.CreateResponsePayment(multipagosResponse,ResponsePaymentConstants.RESPONSEPAYMENT_TYPE_S2S);

                EndPayment endPayment = await _endPaymentService.SendEndPaymentToTibco(responsePaymentId);
            }
            catch(Exception ex)
            {

            }
        }
    }
}
