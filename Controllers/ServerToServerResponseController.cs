using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EPWebAPI.Models;
using EPWebAPI.Interfaces;
using EPWebAPI.Helpers;

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

        public ServerToServerResponseController(ILogPaymentRepository logPaymentRepository,IResponsePaymentRepository responsePaymentRepository,IEndPaymentRepository endPaymentRepository,IResponseBankRequestTypeTibcoRepository responseBankRequestTypeTibcoRepository)
        {
            _logPaymentRepository = logPaymentRepository;
            _responsePaymentRepository = responsePaymentRepository;
            _endPaymentRepository = endPaymentRepository;
            _responseBankRequestTypeTibcoRepository = responseBankRequestTypeTibcoRepository;
        }
        
        // POST api/values
        [HttpPost]
        public async void Post([FromForm] MultiPagosResponsePaymentDTO multipagosResponse)
        {

         var validHash = ServerToServerResponsePaymentHelper.ValidateMultipagosHash(multipagosResponse); 

         var logPayment = _logPaymentRepository.GetLastRequestPaymentId(
            multipagosResponse.mp_amount,
            multipagosResponse.mp_order,
            multipagosResponse.mp_reference,
            "REQUEST_PAYMENT"
        );

        if(validHash)
        {

            var responsePaymentDTO = ServerToServerResponsePaymentHelper.GenerateResponsePaymentDTO(
                multipagosResponse,
                logPayment.RequestPaymentId,
                "HASH_VALIDO"
            );

            int responsePaymentId = _responsePaymentRepository.CreateResponsePayment(responsePaymentDTO);

            var endPayment = _endPaymentRepository.GetEndPaymentByResponsePaymentId(responsePaymentId);

                  if (_endPaymentRepository.ValidateEndPaymentSentStatus(endPayment.EndPaymentId) != true)
                {
                    string resultMessage = await _responseBankRequestTypeTibcoRepository.SendEndPaymentToTibco(endPayment);
                    //Si la respuesta fue satisfactoria actualiza el estatus de endpayment en bd a enviado.
                    if(resultMessage == "OK")
                    {
                        var udpatedEndPaymentId = _endPaymentRepository.UpdateEndPaymentSentStatus(endPayment.EndPaymentId, "ENVIADO_TIBCO");
                    }
                }
             
        }else {
                var responsePaymentDTO = ServerToServerResponsePaymentHelper.GenerateResponsePaymentDTO(
                         multipagosResponse,
                         logPayment.RequestPaymentId,
                         "HASH_INVALIDO");

                //3.- guardar el responsepayment en base de datos (endpayment se guarda a su vez con trigger)
                int responsePaymentId = _responsePaymentRepository.CreateResponsePayment(responsePaymentDTO);
            }

        }
    }
}
