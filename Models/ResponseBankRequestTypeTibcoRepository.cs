using System.Threading.Tasks;
using EPWebAPI.Interfaces;
using TibcoServiceReference;

namespace EPWebAPI.Models
{
    public class ResponseBankRequestTypeTibcoRepository : IResponseBankRequestTypeTibcoRepository
    { 
      
       public async Task<string> SendEndPaymentToTibco(EndPayment endPayment)
        {
            ResponseBankRequestType request = new ResponseBankRequestType
            {
                UltimosCuatroDigitos = endPayment.CcLastFour,
                Token = endPayment.Token,
                RespuestaBanco = endPayment.ResponseMessage,
                NumeroTransaccion = endPayment.TransactionNumber,
                TipoTarjeta = endPayment.CcType,
                BancoEmisor = endPayment.IssuingBank,
                SeviceRequest = endPayment.ServiceRequest,
                BillingAccount = endPayment.BillingAccount
            };


            ResponseBankClient responsebank = new ResponseBankClient();

            ResponseBankResponse response = await responsebank.ResponseBankAsync(request);

            return response.ResponseBankResponse1.ErrorMessage;
        }
    }
}