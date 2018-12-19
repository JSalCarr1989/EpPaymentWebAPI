using Microsoft.AspNetCore.Mvc;
using EPWebAPI.Models;
using EPWebAPI.Interfaces;
using EPWebAPI.Helpers;
using EPWebAPI.Utilities;
using Microsoft.Extensions.Configuration;

namespace EPWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServerToServerResponseController : ControllerBase
    {
        private readonly ILogPaymentRepository _logPaymentRepository;
        private readonly IResponsePaymentRepository _responsePaymentRepository;
        private readonly IEndPaymentRepository _endPaymentRepository;
        private readonly IResponseBankRequestTypeTibcoRepository _responseBankRequestTypeTibcoRepository;
        private readonly ISentToTibcoRepository _sentToTibcoRepo;
        private readonly IConfiguration _config;
        private readonly IDbLoggerRepository _dbLoggerRepository;

        public ServerToServerResponseController(
                            ILogPaymentRepository logPaymentRepository,
                            IResponsePaymentRepository responsePaymentRepository,
                            IEndPaymentRepository endPaymentRepository,
                            IResponseBankRequestTypeTibcoRepository responseBankRequestTypeTibcoRepository, 
                            ISentToTibcoRepository sentToTibcoRepo,
                            IConfiguration config,
                            IDbLoggerRepository dbLoggerRepository
            )
        {
            _logPaymentRepository = logPaymentRepository;
            _responsePaymentRepository = responsePaymentRepository;
            _endPaymentRepository = endPaymentRepository;
            _responseBankRequestTypeTibcoRepository = responseBankRequestTypeTibcoRepository;
            _sentToTibcoRepo = sentToTibcoRepo;
            _dbLoggerRepository = dbLoggerRepository;
            _config = config;
        }
        
        // POST api/values
        [HttpPost]
        public async void Post([FromForm] MultiPagosResponsePaymentDTO multipagosResponse)
        {

            _dbLoggerRepository.LogResponsedDataToDb(multipagosResponse);

            var validHash = ServerToServerResponsePaymentHelper.ValidateMultipagosHash(
                            multipagosResponse,
                            _config,
                            _dbLoggerRepository
                            ); 

            var hashStatus = (validHash) ? StaticResponsePaymentProperties.VALID_HASH 
                                         : StaticResponsePaymentProperties.INVALID_HASH;

            var logPayment = _logPaymentRepository.GetLastRequestPaymentId(
                multipagosResponse.mp_amount,
                multipagosResponse.mp_order,
                multipagosResponse.mp_reference,
                StaticResponsePaymentProperties.REQUEST_PAYMENT_STATUS
                );


            var responsePaymentDTO = ServerToServerResponsePaymentHelper.GenerateResponsePaymentDTO(
                multipagosResponse,
                logPayment.RequestPaymentId,
                hashStatus
            );

            int responsePaymentId = _responsePaymentRepository.CreateResponsePayment(responsePaymentDTO);


            var sentExists = _sentToTibcoRepo.GetEndPaymentSentToTibco(
                             StaticResponsePaymentProperties.ENDPAYMENT_SENTED_STATUS,
                             StaticResponsePaymentProperties.RESPONSEPAYMENT_TYPE_POST, 
                             responsePaymentId
                             );

            var endPayment = _endPaymentRepository.GetEndPaymentByResponsePaymentId(responsePaymentId);

            if (sentExists != true)
            {
                    string resultMessage = await _responseBankRequestTypeTibcoRepository.SendEndPaymentToTibco(endPayment);

                if (resultMessage == StaticResponsePaymentProperties.TIBCO_OK_RESULT_MESSAGE)
                {
                    _endPaymentRepository.UpdateEndPaymentSentStatus(endPayment.EndPaymentId, StaticResponsePaymentProperties.ENDPAYMENT_SENTED_STATUS);
                }
            }

        }
    }
}
