using System.Threading.Tasks;
using EPWebAPI.Models;

namespace EPWebAPI.Interfaces
{
   public interface IResponseBankRequestTypeTibcoRepository
    {
        Task<string> SendEndPaymentToTibco(EndPayment endPayment);
    }
}
