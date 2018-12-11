using EPWebAPI.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using Serilog;
using Serilog.Formatting.Compact;
using System;

namespace EPWebAPI.Models
{
    public class ResponsePaymentRepository : IResponsePaymentRepository
    {

        private IConfiguration _config;
        private readonly string _connectionString;
        private readonly ILogger _logger;

        public ResponsePaymentRepository(IConfiguration config)
        {
            _config = config;

            var environmentConnectionString = Environment.GetEnvironmentVariable("EpPaymentDevConnectionStringEnvironment", EnvironmentVariableTarget.Machine);

            var connectionString = !string.IsNullOrEmpty(environmentConnectionString)
                                   ? environmentConnectionString
                                   : _config.GetConnectionString("EpPaymentDevConnectionString");

            _connectionString = connectionString;

            var logger = new LoggerConfiguration()
                        .MinimumLevel.Debug()
                        .WriteTo.RollingFile(new CompactJsonFormatter(),
                                              @"E:\LOG\EnterprisePaymentLog.json",
                                              shared: true,
                                              retainedFileCountLimit: 30
                                              )
                       .CreateLogger();
            _logger = logger;
        }

        public IDbConnection Connection
        {
            get
            {
                return new SqlConnection(_connectionString);
            }
        }

        public int CreateResponsePayment(ResponsePaymentDTO responseDTO)
        {
            int responsePaymentId;
            using (IDbConnection conn = Connection)
            {
                var parameters = new DynamicParameters();

                parameters.Add("@MP_ORDER", responseDTO.MpOrder);
                parameters.Add("@MP_REFERENCE", responseDTO.MpReference);
                parameters.Add("@MP_AMOUNT", responseDTO.MpAmount);
                parameters.Add("@MP_PAYMENT_METHOD", responseDTO.MpPaymentMethod);
                parameters.Add("@MP_RESPONSE", responseDTO.MpResponse);
                parameters.Add("@MP_RESPONSE_MSG", responseDTO.MpResponseMsg);
                parameters.Add("@MP_AUTHORIZATION", responseDTO.MpAuthorization);
                parameters.Add("@MP_SIGNATURE", responseDTO.MpSignature);
                parameters.Add("@MP_PAN", responseDTO.MpPan);
                parameters.Add("@MP_DATE", responseDTO.MpDate);
                parameters.Add("@MP_BANK_NAME", responseDTO.MpBankName);
                parameters.Add("@MP_FOLIO", responseDTO.MpFolio);
                parameters.Add("@MP_SB_TOKEN", responseDTO.MpSbToken);
                parameters.Add("@MP_SALE_ID", responseDTO.MpSaleId);
                parameters.Add("@MP_CARHOLDER_NAME", responseDTO.MpCardHolderName);
                parameters.Add("@RESPONSE_PAYMENT_TYPE_LIST_DESCRIPTION", responseDTO.ResponsePaymentTypeDescription);
                parameters.Add("@RESPONSE_PAYMENT_HASH_STATUS_DESCRIPTION", responseDTO.ResponsePaymentHashStatusDescription);
                parameters.Add("@REQUEST_PAYMENT_ID", responseDTO.RequestPaymentId);
                parameters.Add("@RESPONSE_PAYMENT_ID",
                                dbType: DbType.Int32,
                                direction: ParameterDirection.Output);

                conn.Open();

                conn.Query("SP_CREATE_RESPONSE_ENTERPRISE_PAYMENT",parameters,commandType: CommandType.StoredProcedure);


                responsePaymentId = parameters.Get<int>("RESPONSE_PAYMENT_ID");

                return responsePaymentId;

            }
        }
    }


    
}
