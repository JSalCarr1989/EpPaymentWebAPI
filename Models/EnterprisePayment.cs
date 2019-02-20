using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPWebAPI.Models
{
    public class EnterprisePayment
    {

        public string CuentaFacturacion { get; set; }
        public string ServiceRequest { get; set; }
        public string ReferenciaPago { get; set; }
        public decimal Monto { get; set; }
        public List<EnterprisePaymentDetail> PaymentDetails {get;set;}

    }
}
