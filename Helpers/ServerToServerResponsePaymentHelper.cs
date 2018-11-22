using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EPWebAPI.Models;
using System.Text;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace EPWebAPI.Helpers
{

    public static class ServerToServerResponsePaymentHelper
    {

      private static string ComputeSha256Hash(string rawData,string secret)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                secret = secret ?? "";
                var encoding = new System.Text.ASCIIEncoding();
                byte[] keyByte = encoding.GetBytes(secret);
                byte[] messageBytes = encoding.GetBytes(rawData);

                using (var hmacsha256 = new HMACSHA256(keyByte))
                {
                    byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
                    return Convert.ToBase64String(hashmessage);
                }
            }
        }

      public static Boolean ValidateMultipagosHash(MultiPagosResponsePaymentDTO multipagosResponse)
        {
            var rawData = multipagosResponse.mp_order + multipagosResponse.mp_reference + multipagosResponse.mp_amount + multipagosResponse.mp_authorization;
            var myHash = ComputeSha256Hash(rawData,"secretkey");

            if(myHash == multipagosResponse.mp_signature)
            {

                return true;

            }

            return false;
        }

        public static ResponsePaymentDTO GenerateResponsePaymentDTO(MultiPagosResponsePaymentDTO multiPagosResponse,int requestPaymentId, string hashStatus)
        {
            ResponsePaymentDTO _responsePaymentDTO = new ResponsePaymentDTO
            {
                MpOrder = multiPagosResponse.mp_order,
                MpReference = multiPagosResponse.mp_reference,
                MpAmount = multiPagosResponse.mp_amount,
                MpPaymentMethod = multiPagosResponse.mp_paymentmethod,
                MpResponse = multiPagosResponse.mp_response,
                MpResponseMsg = multiPagosResponse.mp_responsemsg,
                MpAuthorization = multiPagosResponse.mp_authorization,
                MpSignature = multiPagosResponse.mp_signature,
                MpPan = multiPagosResponse.mp_pan,
                MpDate = multiPagosResponse.mp_date,
                MpBankName = multiPagosResponse.mp_bankname,
                MpFolio = multiPagosResponse.mp_folio,
                MpSbToken = multiPagosResponse.mp_sbtoken,
                MpSaleId = multiPagosResponse.mp_saleid,
                MpCardHolderName = multiPagosResponse.mp_cardholdername,
                ResponsePaymentTypeDescription = "MULTIPAGOS_SERVER2SERVER",
                ResponsePaymentHashStatusDescription = hashStatus,
                RequestPaymentId = requestPaymentId
            };

            return _responsePaymentDTO;
        }
    }

}