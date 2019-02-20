using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EPWebAPI.Models;

namespace EPWebAPI.Interfaces
{
    public interface IEnterprisePaymentMonitorRepository
    {
         IEnumerable<EnterprisePayment> GetAllPayments();
    }
}
