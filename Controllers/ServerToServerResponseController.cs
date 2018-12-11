using Microsoft.AspNetCore.Mvc;
using EPWebAPI.Models;
using EPWebAPI.Interfaces;
using EPWebAPI.Helpers;
using Serilog;
using Serilog.Formatting.Compact;
using System.IO;
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
        private readonly ILogger _logger;
        private readonly IConfiguration _config;

        public ServerToServerResponseController(ILogPaymentRepository logPaymentRepository,IResponsePaymentRepository responsePaymentRepository,IEndPaymentRepository endPaymentRepository,IResponseBankRequestTypeTibcoRepository responseBankRequestTypeTibcoRepository, ISentToTibcoRepository sentToTibcoRepo,IConfiguration config)
        {
            _logPaymentRepository = logPaymentRepository;
            _responsePaymentRepository = responsePaymentRepository;
            _endPaymentRepository = endPaymentRepository;
            _responseBankRequestTypeTibcoRepository = responseBankRequestTypeTibcoRepository;
            _sentToTibcoRepo = sentToTibcoRepo;

            var logger = new LoggerConfiguration()
.MinimumLevel.Information()
.WriteTo.RollingFile(new CompactJsonFormatter(),
                     //@"E:\LOG\EnterprisePaymentLog.json",
                     Path.Combine(@"E:\LOG\", @"EnterprisePaymentLog.json"),
                     shared: true,
                     retainedFileCountLimit: 30
                     )
.CreateLogger();
            _logger = logger;
            _config = config;

        }
        
        // POST api/values
        [HttpPost]
        public async void Post([FromForm] MultiPagosResponsePaymentDTO multipagosResponse)
        {

         var validHash = ServerToServerResponsePaymentHelper.ValidateMultipagosHash(multipagosResponse,_config); 

         var hashStatus = (validHash) ? "HASH_VALIDO" : "HASH_INVALIDO";

         var logPayment = _logPaymentRepository.GetLastRequestPaymentId(
            multipagosResponse.mp_amount,
            multipagosResponse.mp_order,
            multipagosResponse.mp_reference,
            "REQUEST_PAYMENT"
        );

            var responsePaymentDTO = ServerToServerResponsePaymentHelper.GenerateResponsePaymentDTO(
                multipagosResponse,
                logPayment.RequestPaymentId,
                hashStatus
            );

            int responsePaymentId = _responsePaymentRepository.CreateResponsePayment(responsePaymentDTO);

            var sentExists = _sentToTibcoRepo.GetEndPaymentSentToTibco("ENVIADO_TIBCO", "MULTIPAGOS_POST", responsePaymentId);

            var endPayment = _endPaymentRepository.GetEndPaymentByResponsePaymentId(responsePaymentId);

            if(sentExists != true)
            {
                    string resultMessage = await _responseBankRequestTypeTibcoRepository.SendEndPaymentToTibco(endPayment);

                    //Si la respuesta fue satisfactoria actualiza el estatus de endpayment en bd a enviado.
                    if(resultMessage == "OK")
                    {
                    _logger.Information(
    "result message: {resultMessage}",
    resultMessage);
                    var udpatedEndPaymentId = _endPaymentRepository.UpdateEndPaymentSentStatus(endPayment.EndPaymentId, "ENVIADO_TIBCO");
                    }else
                {
                    _logger.Information(
    "result message: {resultMessage}",
    resultMessage);
                }
            }

        }
    }
}
