using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using EPWebAPI.Interfaces;
using Microsoft.Extensions.Configuration;
using Dapper;
using System;
using Serilog;
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
            IDbConnection conn = Connection;
            requestPayment.MpPaymentDatetime = DateTime.Now.ToString(StaticRequestEP.DATETIMEFORMAT);
            try 
            {
            using(conn)
            {
                var parameters = new DynamicParameters();

                parameters.Add(StaticRequestEP.MP_ACCOUNT,requestPayment.MpAccount);
                parameters.Add(StaticRequestEP.MP_PRODUCT,requestPayment.MpProduct);
                parameters.Add(StaticRequestEP.MP_ORDER,requestPayment.MpOrder);
                parameters.Add(StaticRequestEP.MP_REFERENCE,requestPayment.MpReference);
                parameters.Add(StaticRequestEP.MP_NODE,requestPayment.MpNode);
                parameters.Add(StaticRequestEP.MP_CONCEPT,requestPayment.MpConcept);
                parameters.Add(StaticRequestEP.MP_AMOUNT,requestPayment.MpAmount);
                parameters.Add(StaticRequestEP.MP_CUSTOMER_NAME,requestPayment.MpCustomerName);
                parameters.Add(StaticRequestEP.MP_CURRENCY,requestPayment.MpCurrency);
                parameters.Add(StaticRequestEP.MP_SIGNATURE,requestPayment.MpSignature);
                parameters.Add(StaticRequestEP.MP_URL_SUCCESS,requestPayment.MpUrlSuccess);
                parameters.Add(StaticRequestEP.MP_URL_FAILURE,requestPayment.MpUrlFailure);
                parameters.Add(StaticRequestEP.MP_REGISTER_SB,requestPayment.MpRegisterSb);
                parameters.Add(StaticRequestEP.MP_PAYMENTDATETIME,requestPayment.MpPaymentDatetime);
                parameters.Add(StaticRequestEP.BEGIN_PAYMENT_ID,requestPayment.BeginPaymentId);
                parameters.Add(StaticRequestEP.REQUEST_PAYMENT_ID, 
                                    dbType: DbType.Int32, 
                                    direction: ParameterDirection.Output);

                conn.Open();

                _logger.Information( 
                                     StaticRequestEP.LogTemplateBeforeInsert,
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
                                     StaticRequestEP.PaymentStage,
                                     StaticRequestEP.ComunicationStep
                                   );

                await conn.QueryAsync(
                    StaticRequestEP.SP_CREATE_REQUEST_ENTERPRISE_PAYMENT,
                    parameters,
                    commandType:CommandType.StoredProcedure);

                id = parameters.Get<int>(StaticRequestEP.REQUEST_PAYMENT_ID_OUTPUT_SEARCH);

                _logger.Information(
                            StaticRequestEP.LogTemplateAfterInsert,
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
            finally
            {
                conn.Close();
            }
        }

        public async  Task<RequestPayment> GetById(int id)
        {
            using(IDbConnection conn = Connection)
            {
                conn.Open();
                var result = await conn.QueryFirstOrDefaultAsync<RequestPayment>(StaticRequestEP.SP_EP_GET_REQUESTPAYMENT_BY_ID,new {RequestPaymentId = id},commandType:CommandType.StoredProcedure);
                return result;
            }
        }
    }
}