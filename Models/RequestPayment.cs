namespace EPWebAPI.Models 
{
    public class RequestPayment {
        public int RequestPaymentId {get;set;}
        public string MpAccount {get;set;}
        public int MpProduct {get;set;}
        public string MpOrder {get;set;}
        public string MpReference {get;set;}
        public string MpNode {get;set;}
        public string MpConcept {get;set;}
        public decimal MpAmount {get;set;}
        public string MpCustomerName {get;set;}
        public int MpCurrency {get;set;}
        public string MpSignature {get;set;}
        public string MpUrlSuccess {get;set;}
        public string MpUrlFailure {get;set;}
        public string MpRegisterSb {get;set;}
        public string MpPaymentDatetime {get;set;}

        public int BeginPaymentId {get;set;}

    }
}