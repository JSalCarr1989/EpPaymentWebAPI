using EPWebAPI.Models;

namespace EPWebAPI.Interfaces
{
   public interface IResponsePaymentDTORepository
    {
         ResponsePaymentDTO GenerateResponsePaymentDTO(MultiPagosResponsePaymentDTO multiPagosResponse, int requestPaymentId, string hashStatus);
    }
}
