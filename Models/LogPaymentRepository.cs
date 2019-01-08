using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using EPWebAPI.Interfaces;
using EPWebAPI.Utilities;
using Dapper;
using System;

namespace EPWebAPI.Models 
{
    public class LogPaymentRepository : ILogPaymentRepository
    {
       
        private readonly IDbLoggerRepository _dbLoggerRepository;
        private readonly IDbLoggerErrorRepository _dbLoggerErrorRepository;
        private readonly IDbConnectionRepository _connectionRepository;
        private readonly IDbConnection _conn;

        public LogPaymentRepository(
                                    IDbLoggerRepository dbLoggerRepository,
                                    IDbLoggerErrorRepository dbLoggerErrorRepository,
                                    IDbConnectionRepository connectionRepository
                                  )
        {
            _connectionRepository = connectionRepository;
            _dbLoggerRepository = dbLoggerRepository;
            _dbLoggerErrorRepository = dbLoggerErrorRepository;
            _conn = connectionRepository.CreateDbConnection();
        }

        public async Task<List<LogPayment>> GetByServiceRequestAndPaymentReference(
             string serviceRequest, 
             string paymentReference
            )
        {

            using(_conn)
            {
                _conn.Open();
              var result  = await _conn.QueryAsync<LogPayment>(
                            StaticLogPaymentProperties.SP_MONITOR_EP_GET_LOGPAYMENT_BY_SR_PR,
                            new {
                                SERVICE_REQUEST = serviceRequest,
                                PAYMENT_REFERENCE = paymentReference
                                },
                            commandType:CommandType.StoredProcedure
                            );
                return result.ToList();  
            }

             
        }

        public LogPayment GetLastRequestPaymentId(
                           decimal amount, 
                           string serviceRequest, 
                           string paymentReference, 
                           string StatusPayment
            )
        {
            LogPayment result = null;

            try
            {
                using (_conn)
                {
                    var parameters = new DynamicParameters();
                    parameters.Add(StaticLogPaymentProperties.PAYMENT_REQUEST_AMOUNT, amount);
                    parameters.Add(StaticLogPaymentProperties.SERVICE_REQUEST, serviceRequest);
                    parameters.Add(StaticLogPaymentProperties.PAYMENT_REFERENCE, paymentReference);
                    parameters.Add(StaticLogPaymentProperties.STATUS_PAYMENT, StatusPayment);

                    _conn.Open();

                    result = _conn.QueryFirstOrDefault<LogPayment>(
                        StaticLogPaymentProperties.GET_LAST_REQUESTPAYMENT_ID,
                        parameters,
                        commandType: CommandType.StoredProcedure
                        );

                    _dbLoggerRepository.LogGetLastRequestPaymentId(amount, serviceRequest, paymentReference, StatusPayment, result.RequestPaymentId);

                    
                }
            }
            catch(Exception ex)
            {
                _dbLoggerErrorRepository.LogGetLastRequestPaymentIdError(ex.ToString(), serviceRequest, paymentReference);
            }

            return result;
        }
    }
}