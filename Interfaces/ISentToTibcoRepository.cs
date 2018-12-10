using System.Collections.Generic;
using System.Threading.Tasks;
using EPWebAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace EPWebAPI.Interfaces
{

public interface ISentToTibcoRepository 
{

    bool GetEndPaymentSentToTibco(string endPaymentStatusDescription,string responsePaymentType, int responsePaymentId);

}

}