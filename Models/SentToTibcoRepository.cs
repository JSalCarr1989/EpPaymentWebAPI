using System.Data;
using EPWebAPI.Interfaces;
using EPWebAPI.Utilities;
using Dapper;
using System;

namespace EPWebAPI.Models
{
    public class SentToTibcoRepository : ISentToTibcoRepository
    {

        private readonly IDbConnectionRepository _dbConnectionRepository;
        private readonly IDbLoggerRepository _dbLoggerRepository;
        private readonly IDbLoggerErrorRepository _dbLoggerErrorRepository;
        private readonly IDbConnection _conn;

        public SentToTibcoRepository(
                                     IDbConnectionRepository dbConnectionRepository,
                                     IDbLoggerRepository dbLoggerRepository,
                                     IDbLoggerErrorRepository dbLoggerErrorRepository
                                    )
        {
            _dbConnectionRepository = dbConnectionRepository;
            _dbLoggerRepository = dbLoggerRepository;
            _dbLoggerErrorRepository = dbLoggerErrorRepository;
            _conn = _dbConnectionRepository.CreateDbConnection();
        }

        public bool GetEndPaymentSentToTibco(string endPaymentStatusDescription, string responsePaymentType, int responsePaymentId)
        {
            int sentToTibcoCount = 0;
            bool sentToTibco = false;

            try
            {
                using (_conn)
                {

                    var parameters = new DynamicParameters();


                    parameters.Add(StaticSentToTibcoRepositoryProperties.END_PAYMENT_STATUS_DESCRIPTION, endPaymentStatusDescription);
                    parameters.Add(StaticSentToTibcoRepositoryProperties.RESPONSE_PAYMENT_TYPE, responsePaymentType);
                    parameters.Add(StaticSentToTibcoRepositoryProperties.RESPONSE_PAYMENT_ID, responsePaymentId);
                    parameters.Add(StaticSentToTibcoRepositoryProperties.SENT_NUM,
        dbType: DbType.Int32,
        direction: ParameterDirection.Output);

                    _conn.Open();

                    _conn.Query(StaticSentToTibcoRepositoryProperties.SP_GET_SENT_TO_TIBCO, parameters, commandType: CommandType.StoredProcedure);

                    sentToTibcoCount = parameters.Get<int>(StaticSentToTibcoRepositoryProperties.SENT_NUM_OUTPUT_SEARCH);

                    sentToTibco = (sentToTibcoCount > 0) ? true : false;

                    _dbLoggerRepository.LogGetSentExists(endPaymentStatusDescription, responsePaymentType, responsePaymentId, sentToTibco);

                }
            }
            catch(Exception ex)
            {
                _dbLoggerErrorRepository.LogGetEndPaymentSentToTibcoError(ex.ToString());
            }

            return sentToTibco;
        }
    }
}
