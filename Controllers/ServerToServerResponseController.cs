using Microsoft.AspNetCore.Mvc;
using EPWebAPI.Models;
using EPWebAPI.Interfaces;
using EPWebAPI.Helpers;
using Serilog;
using Serilog.Formatting.Compact;
using System.IO;
using Microsoft.Extensions.Configuration;
using System;

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
        private readonly ILogger _logger;
        private readonly IConfiguration _config;

        public ServerToServerResponseController(ILogPaymentRepository logPaymentRepository,IResponsePaymentRepository responsePaymentRepository,IEndPaymentRepository endPaymentRepository,IResponseBankRequestTypeTibcoRepository responseBankRequestTypeTibcoRepository, ISentToTibcoRepository sentToTibcoRepo,IConfiguration config)
        {
            _logPaymentRepository = logPaymentRepository;
            _responsePaymentRepository = responsePaymentRepository;
            _endPaymentRepository = endPaymentRepository;
            _responseBankRequestTypeTibcoRepository = responseBankRequestTypeTibcoRepository;
            _sentToTibcoRepo = sentToTibcoRepo;

            var environmentConnectionString = Environment.GetEnvironmentVariable("EpPaymentDevConnectionStringEnvironment", EnvironmentVariableTarget.Machine);

            var connectionString = !string.IsNullOrEmpty(environmentConnectionString)
                       ? environmentConnectionString
                       : _config.GetConnectionString("EpPaymentDevConnectionString");

            var logger = new LoggerConfiguration()
.MinimumLevel.Information()
.WriteTo.MSSqlServer(connectionString, "Log")
.CreateLogger();
            _logger = logger;
            _config = config;

        }
        
        // POST api/values
        [HttpPost]
        public async void Post([FromForm] MultiPagosResponsePaymentDTO multipagosResponse)
        {

            EnterprisePaymentDbLogHelpers.LogResponsedDataToDb(
                       _logger,
                       multipagosResponse
                       );

            var validHash = ServerToServerResponsePaymentHelper.ValidateMultipagosHash(
                            multipagosResponse,
                            _config
                            ); 

            var hashStatus = (validHash) ? "HASH_VALIDO" : "HASH_INVALIDO";

            var logPayment = _logPaymentRepository.GetLastRequestPaymentId(
                multipagosResponse.mp_amount,
                multipagosResponse.mp_order,
                multipagosResponse.mp_reference,
                "REQUEST_PAYMENT"
                );

            EnterprisePaymentDbLogHelpers.LogGetLastRequestPaymentId(
                     _logger,
                     multipagosResponse,
                    "REQUEST_PAYMENT",
                    logPayment.RequestPaymentId
                   );

            var responsePaymentDTO = ServerToServerResponsePaymentHelper.GenerateResponsePaymentDTO(
                multipagosResponse,
                logPayment.RequestPaymentId,
                hashStatus
            );

            int responsePaymentId = _responsePaymentRepository.CreateResponsePayment(responsePaymentDTO);

            EnterprisePaymentDbLogHelpers.LogCreateResponsePayment(
                      _logger,
                      responsePaymentDTO,
                      responsePaymentId
                      );

            var sentExists = _sentToTibcoRepo.GetEndPaymentSentToTibco(
                             "ENVIADO_TIBCO", 
                             "MULTIPAGOS_POST", 
                             responsePaymentId
                             );

            EnterprisePaymentDbLogHelpers.LogGetSentExists(
                            _logger,
                           "ENVIADO_TIBCO",
                           "MULTIPAGOS_SERVER2SERVER",
                           responsePaymentId,
                           sentExists
                           );

            var endPayment = _endPaymentRepository.GetEndPaymentByResponsePaymentId(responsePaymentId);

            EnterprisePaymentDbLogHelpers.LogGetEndPayment(
                      _logger,
                      responsePaymentId,
                      endPayment
                      );

            if (sentExists != true)
            {
                    string resultMessage = await _responseBankRequestTypeTibcoRepository.SendEndPaymentToTibco(endPayment,_logger);

                    
                    if(resultMessage == "OK")
                    {
                    _endPaymentRepository.UpdateEndPaymentSentStatus(endPayment.EndPaymentId, "ENVIADO_TIBCO");

                    EnterprisePaymentDbLogHelpers.LogUpdateEndPaymentSentStatus(
                             _logger,
                            endPayment.EndPaymentId,
                            "ENVIADO_TIBCO"
                           );

                }
            }

        }
    }
}
