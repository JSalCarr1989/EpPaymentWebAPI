using EPWebAPI.Models;

namespace EPWebAPI.Interfaces
{
    public interface IResponsePaymentRepository
    {
        int CreateResponsePayment(ResponsePaymentDTO responseDTO);
    }
}