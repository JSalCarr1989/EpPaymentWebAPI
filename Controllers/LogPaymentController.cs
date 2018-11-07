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
    public class LogPaymentController : ControllerBase
    {
        private readonly ILogPaymentRepository _logPaymentRepo;

        public LogPaymentController(ILogPaymentRepository logPaymentRepo)
        {
            _logPaymentRepo = logPaymentRepo;
        }
        
        [HttpGet("{serviceRequest}/{paymentReference}")]
        public async Task<List<LogPayment>> GetByServiceRequestAndPaymentReference(string serviceRequest,string paymentReference)
        {
           return await _logPaymentRepo.GetByServiceRequestAndPaymentReference(serviceRequest,paymentReference);
        }
    }
}