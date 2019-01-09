using System;

namespace EPWebAPI.Models
{
    public class ResponsePaymentDTO
    {
        public int ResponsePaymentId { get; set; }
        public string MpOrder { get; set; }
        public string MpReference { get; set; }
        public decimal MpAmount { get; set; }
        public string MpPaymentMethod { get; set; }
        public string MpResponse { get; set; }
        public string MpResponseMsg { get; set; }
        public string MpAuthorization { get; set; }
        public string MpSignature { get; set; }
        public string MpPan { get; set; }
        public DateTime MpDate { get; set; }
        public string MpBankName { get; set; }
        public string MpFolio { get; set; }
        public string MpSbToken { get; set; }
        public int MpSaleId { get; set; }
        public string MpCardHolderName { get; set; }
        public string ResponsePaymentTypeDescription { get; set; }
        public string ResponsePaymentHashStatusDescription { get; set; }
        public int RequestPaymentId { get; set; }
    }
}