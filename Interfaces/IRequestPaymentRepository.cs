using System.Collections.Generic;
using System.Threading.Tasks;
using EPWebAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace EPWebAPI.Interfaces
{
    public interface IRequestPaymentRepository
    {
        Task<int> Create(RequestPayment requestPayment);
        Task<RequestPayment> GetById(int id);
    }
    
}