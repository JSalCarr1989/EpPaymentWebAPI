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
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;
using EPWebAPI.Utilities;

namespace EPWebAPI.Models {
    public class RequestPaymentRepository : IRequestPaymentRepository
    {
        private readonly IConfiguration _config;
        private readonly ILogger _logger;
       
       

        public RequestPaymentRepository(IConfiguration config)
        {
            var logger = new LoggerConfiguration()
                         .MinimumLevel.Debug()
                         .WriteTo.RollingFile( new CompactJsonFormatter(),
                                               @"E:\LOG\EnterprisePaymentLog.json",
                                               shared:true,
                                               retainedFileCountLimit:30
                                               )
                        .CreateLogger();
            _logger = logger;
            _config = config;
        }

        public IDbConnection Connection 
        {
            get 
            {
                return new SqlConnection(_config.GetConnectionString("EpWebAPIConnectionString"));
            }
        }
        public async Task<int> Create(RequestPayment requestPayment)
        {
            int id;
            requestPayment.MpPaymentDatetime = DateTime.Now.ToString(StaticRequestEnterprisePayment.DATETIMEFORMAT);
            try 
            {
            using(IDbConnection conn = Connection)
            {
                var parameters = new DynamicParameters();

                parameters.Add(StaticRequestEnterprisePayment.MP_ACCOUNT,requestPayment.MpAccount);
                parameters.Add(StaticRequestEnterprisePayment.MP_PRODUCT,requestPayment.MpProduct);
                parameters.Add(StaticRequestEnterprisePayment.MP_ORDER,requestPayment.MpOrder);
                parameters.Add(StaticRequestEnterprisePayment.MP_REFERENCE,requestPayment.MpReference);
                parameters.Add(StaticRequestEnterprisePayment.MP_NODE,requestPayment.MpNode);
                parameters.Add(StaticRequestEnterprisePayment.MP_CONCEPT,requestPayment.MpConcept);
                parameters.Add(StaticRequestEnterprisePayment.MP_AMOUNT,requestPayment.MpAmount);
                parameters.Add(StaticRequestEnterprisePayment.MP_CUSTOMER_NAME,requestPayment.MpCustomerName);
                parameters.Add(StaticRequestEnterprisePayment.MP_CURRENCY,requestPayment.MpCurrency);
                parameters.Add(StaticRequestEnterprisePayment.MP_SIGNATURE,requestPayment.MpSignature);
                parameters.Add(StaticRequestEnterprisePayment.MP_URL_SUCCESS,requestPayment.MpUrlSuccess);
                parameters.Add(StaticRequestEnterprisePayment.MP_URL_FAILURE,requestPayment.MpUrlFailure);
                parameters.Add(StaticRequestEnterprisePayment.MP_REGISTER_SB,requestPayment.MpRegisterSb);
                parameters.Add(StaticRequestEnterprisePayment.MP_PAYMENTDATETIME,requestPayment.MpPaymentDatetime);
                parameters.Add(StaticRequestEnterprisePayment.REQUEST_PAYMENT_ID, 
                                    dbType: DbType.Int32, 
                                    direction: ParameterDirection.Output);

                conn.Open();

                _logger.Information( 
                                     StaticRequestEnterprisePayment.LogTemplateBeforeInsert,
                                     requestPayment.MpAccount,
                                     requestPayment.MpProduct,
                                     requestPayment.MpOrder,
                                     requestPayment.MpReference,
                                     requestPayment.MpNode,
                                     requestPayment.MpConcept,
                                     requestPayment.MpAmount,
                                     requestPayment.MpCustomerName,
                                     requestPayment.MpCurrency,
                                     requestPayment.MpSignature,
                                     requestPayment.MpUrlSuccess,
                                     requestPayment.MpUrlFailure,
                                     requestPayment.MpRegisterSb,
                                     requestPayment.MpPaymentDatetime,
                                     StaticRequestEnterprisePayment.PaymentStage,
                                     StaticRequestEnterprisePayment.ComunicationStep
                                   );

                await conn.QueryAsync(
                    StaticRequestEnterprisePayment.SP_CREATE_REQUEST_ENTERPRISE_PAYMENT,
                    parameters,
                    commandType:CommandType.StoredProcedure);

                id = parameters.Get<int>(StaticRequestEnterprisePayment.REQUEST_PAYMENT_ID_OUTPUT_SEARCH);

                _logger.Information(
                            StaticRequestEnterprisePayment.LogTemplateAfterInsert,
                            id,
                            requestPayment.MpOrder,
                            requestPayment.MpReference
                );

                 return id;
            }
            }
            catch(Exception ex)
            {
               _logger.Error(ex.ToString(),$"Failed Operation for serviceRequest:{requestPayment.MpOrder}");
               return 0;
            }
        }

        public async  Task<RequestPayment> GetById(int id)
        {
            using(IDbConnection conn = Connection)
            {
                conn.Open();
                var result = await conn.QueryFirstOrDefaultAsync<RequestPayment>(StaticRequestEnterprisePayment.SP_EP_GET_REQUESTPAYMENT_BY_ID,new {RequestPaymentId = id},commandType:CommandType.StoredProcedure);
                return result;
            }
        }
    }
}