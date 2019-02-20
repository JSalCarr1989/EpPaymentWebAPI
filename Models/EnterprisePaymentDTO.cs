using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPWebAPI.Models
{
    public class EnterprisePaymentDTO
    {
        public int ResponsePaymentId { get; set; }
        public string CuentaFacturacion { get; set; }
        public string ServiceRequest { get; set; }
        public string PaymentReference { get; set; }
        public decimal Monto { get; set; }
        public string TipoPago { get; set; }
        public string CodigoRespuesta { get; set; }
        public string MensajeRespuesta { get; set; }
        public string AutorizacionBancaria { get; set; }
        public int IdVentaMultipagos { get; set; }
        public string OrigenRespuesta { get; set; }
        public string EstatusHash { get; set; }
        public string UltimosCuatroDigitos { get; set; }
        public string Bin { get; set; }
        public string TipoTc { get; set; }
        public string BancoEmisor { get; set; }
        public string EstatusEnviadoTibco { get; set; }
    }
}
