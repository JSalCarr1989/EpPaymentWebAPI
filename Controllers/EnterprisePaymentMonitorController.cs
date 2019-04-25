using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using EPPCIDAL.Interfaces;
using EPPCIDAL.Models;

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
