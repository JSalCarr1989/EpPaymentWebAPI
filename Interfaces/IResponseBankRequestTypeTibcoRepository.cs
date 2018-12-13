using System.Threading.Tasks;
using EPWebAPI.Models;
using Serilog;

namespace EPWebAPI.Interfaces
{
   public interface IResponseBankRequestTypeTibcoRepository
    {
        Task<string> SendEndPaymentToTibco(EndPayment endPayment, ILogger log);
    }
}
