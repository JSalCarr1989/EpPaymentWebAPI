using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EPPCIDAL.Interfaces;
using EPPCIDAL.Models;


namespace EPWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogPaymentController : ControllerBase
    {
        
        private readonly ILogPaymentService _logPaymentService;

        public LogPaymentController(ILogPaymentService logPaymentService)
        {
            
            _logPaymentService = logPaymentService;
        }
        
        [HttpGet("{serviceRequest}/{paymentReference}")]
        public async Task<List<LogPayment>> GetByServiceRequestAndPaymentReference(string serviceRequest,string paymentReference)
        {
            return await _logPaymentService.GetByServiceRequestAndPaymentReference(serviceRequest, paymentReference);
        }
    }
}