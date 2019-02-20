using Microsoft.AspNetCore.Mvc;
using EPWebAPI.Models;
using EPWebAPI.Interfaces;
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
        private readonly IDbLoggerErrorRepository _dbLoggerErrorRepository;
        private readonly IHashRepository _hashRepository;
        private readonly IResponsePaymentDTORepository _responsePaymentDTORepository;

        public ServerToServerResponseController(
                            ILogPaymentRepository logPaymentRepository,
                            IResponsePaymentRepository responsePaymentRepository,
                            IEndPaymentRepository endPaymentRepository,
                            IResponseBankRequestTypeTibcoRepository responseBankRequestTypeTibcoRepository, 
                            ISentToTibcoRepository sentToTibcoRepo,
                            IConfiguration config,
                            IDbLoggerRepository dbLoggerRepository,
                            IDbLoggerErrorRepository dbLoggerErrorRepository,
                            IHashRepository hashRepository,
                            IResponsePaymentDTORepository responsePaymentDTORepository
            )
        {
            _logPaymentRepository = logPaymentRepository;
            _responsePaymentRepository = responsePaymentRepository;
            _endPaymentRepository = endPaymentRepository;
            _responseBankRequestTypeTibcoRepository = responseBankRequestTypeTibcoRepository;
            _sentToTibcoRepo = sentToTibcoRepo;
            _dbLoggerRepository = dbLoggerRepository;
            _dbLoggerErrorRepository = dbLoggerErrorRepository;
            _config = config;
            _hashRepository = hashRepository;
            _responsePaymentDTORepository = responsePaymentDTORepository;
        }
        
        // POST api/values
        [HttpPost]
        public async void Post([FromForm] MultiPagosResponsePaymentDTO multipagosResponse)
        {

            _dbLoggerRepository.LogResponsedDataToDb(multipagosResponse);


            string hashStatus = _hashRepository.GetHashStatus(multipagosResponse);

            LogPayment logPayment = _logPaymentRepository.GetLastRequestPaymentId(
                multipagosResponse.mp_amount,
                multipagosResponse.mp_order,
                multipagosResponse.mp_reference,
                StaticResponsePaymentProperties.REQUEST_PAYMENT_STATUS
                );


            ResponsePaymentDTO responsePaymentDTO = _responsePaymentDTORepository.GenerateResponsePaymentDTO(multipagosResponse, logPayment.RequestPaymentId, hashStatus);

            int responsePaymentId = _responsePaymentRepository.CreateResponsePayment(responsePaymentDTO);


            bool sentExists = _sentToTibcoRepo.GetEndPaymentSentToTibco(
                             StaticResponsePaymentProperties.ENDPAYMENT_SENTED_STATUS,
                             StaticResponsePaymentProperties.RESPONSEPAYMENT_TYPE_POST, 
                             responsePaymentId
                             );

            EndPayment endPayment = _endPaymentRepository.GetEndPaymentByResponsePaymentId(responsePaymentId);

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
