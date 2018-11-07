using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using EPWebAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Dapper;
using System;
using EPWebAPI.Utilities;

namespace EPWebAPI.Models 
{
    public class LogPaymentRepository : ILogPaymentRepository
    {
        private readonly IConfiguration _config;

        public LogPaymentRepository(IConfiguration config)
        {
           _config = config;
        }

        public IDbConnection Connection
        {
            get 
            {
                return new SqlConnection(_config.GetConnectionString("EpWebAPIConnectionString"));
            }
        }

        public async Task<List<LogPayment>> GetByServiceRequestAndPaymentReference(string serviceRequest, string paymentReference)
        {

            using(IDbConnection conn = Connection)
            {
               conn.Open();
              var result  = await conn.QueryAsync<LogPayment>(
                            "SP_MONITOR_EP_GET_LOGPAYMENT_BY_SR_PR",
                            new {
                                SERVICE_REQUEST = serviceRequest,
                                PAYMENT_REFERENCE = paymentReference
                                },
                            commandType:CommandType.StoredProcedure
                            );
                return result.ToList();  
            }

             
        }
    }
}