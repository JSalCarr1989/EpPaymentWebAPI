using System;
using EPWebAPI.Models;

namespace EPWebAPI.Interfaces
{
    public interface IEndPaymentRepository
    {
        EndPayment GetEndPaymentByResponsePaymentId(int responsePaymentId);
        void UpdateEndPaymentSentStatus(int endPaymentId,string endPaymentSentStatus);
    }
}