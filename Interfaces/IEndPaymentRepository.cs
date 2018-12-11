using System;
using EPWebAPI.Models;

namespace EPWebAPI.Interfaces
{
    public interface IEndPaymentRepository
    {
        EndPayment GetEndPaymentByResponsePaymentId(int responsePaymentId);
        Boolean ValidateEndPaymentSentStatus(int endPaymentId);
        void UpdateEndPaymentSentStatus(int endPaymentId,string endPaymentSentStatus);
    }
}