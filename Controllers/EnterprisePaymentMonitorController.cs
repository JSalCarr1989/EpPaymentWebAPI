using EPWebAPI.Interfaces;
using EPWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace EPWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnterprisePaymentMonitorController : ControllerBase
    {
        private readonly IEnterprisePaymentMonitorRepository _enterprisePaymentMonitorRepository;

        public EnterprisePaymentMonitorController(IEnterprisePaymentMonitorRepository enterprisePaymentMonitorRepository)
        {
            _enterprisePaymentMonitorRepository = enterprisePaymentMonitorRepository;
        }

        [HttpGet]
        public IEnumerable<EnterprisePayment> GetAllPayments()
        {
            return _enterprisePaymentMonitorRepository.GetAllPayments();
        }
    }
}
