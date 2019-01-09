using System.Data;
using System.Threading.Tasks;
using EPWebAPI.Interfaces;
using Dapper;
using System;
using EPWebAPI.Utilities;

namespace EPWebAPI.Models {
    public class RequestPaymentRepository : IRequestPaymentRepository
    {

        private readonly IDbConnectionRepository _dbConnectionRepository;
        private readonly IDbLoggerRepository _dbLoggerRepository;
        private readonly IDbLoggerErrorRepository _dbLoggerErrorRepository;
        private readonly IDbConnection _conn;
       
       

        public RequestPaymentRepository(IDbConnectionRepository dbConnectionRepository,
                                        IDbLoggerRepository dbLoggerRepository,
                                        IDbLoggerErrorRepository dbLoggerErrorRepository)
        {
            _dbConnectionRepository = dbConnectionRepository;
            _dbLoggerRepository = dbLoggerRepository;
            _dbLoggerErrorRepository = dbLoggerErrorRepository;
            _conn = _dbConnectionRepository.CreateDbConnection();
        }

        public async Task<int> Create(RequestPayment requestPayment)
        {
            int id = 0;
            
            requestPayment.MpPaymentDatetime = DateTime.Now.ToString(StaticRequestEP.DATETIMEFORMAT);
            try 
            {
            using(_conn)
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

                _conn.Open();

                await _conn.QueryAsync(
                    StaticRequestEP.SP_CREATE_REQUEST_ENTERPRISE_PAYMENT,
                    parameters,
                    commandType:CommandType.StoredProcedure);

                id = parameters.Get<int>(StaticRequestEP.REQUEST_PAYMENT_ID_OUTPUT_SEARCH);

                    _dbLoggerRepository.LogCreateRequestPayment(requestPayment, id);

                 
            }
            }
            catch(Exception ex)
            {
                _dbLoggerErrorRepository.LogCreateRequestPaymentError(ex.ToString(), requestPayment.MpReference, requestPayment.MpOrder);
            }

            return id;
        }

        public async  Task<RequestPayment> GetById(int id)
        {
            using(_conn)
            {
                _conn.Open();
                var result = await _conn.QueryFirstOrDefaultAsync<RequestPayment>(StaticRequestEP.SP_EP_GET_REQUESTPAYMENT_BY_ID,new {RequestPaymentId = id},commandType:CommandType.StoredProcedure);
                return result;
            }
        }
    }
}