

namespace EPWebAPI.Utilities 
{
    public static class StaticRequestEnterprisePayment 
    {
        public static string MP_ACCOUNT 
        {
            get { return "@MP_ACCOUNT";}
        }

        public static string MP_PRODUCT
        {
            get {return "@MP_PRODUCT";}
        }

        public static string MP_ORDER
        {
            get {return "@MP_ORDER";}
        }

        public static string MP_REFERENCE
        {
            get {return "@MP_REFERENCE";}
        }

        public static string MP_NODE
        {
            get {return "@MP_NODE";}
        }

        public static string MP_CONCEPT
        {
            get {return "@MP_CONCEPT";}
        }

        public static string MP_AMOUNT
        {
            get {return "@MP_AMOUNT";}
        }

        public static string MP_CUSTOMER_NAME
        {
            get {return "@MP_CUSTOMER_NAME";}
        }

        public static string MP_CURRENCY
        {
            get {return "@MP_CURRENCY";}
        }

        public static string MP_SIGNATURE
        {
            get {return "@MP_SIGNATURE";}
        }

        public static string MP_URL_SUCCESS
        {
            get {return "@MP_URL_SUCCESS";}
        }

        public static string MP_URL_FAILURE
        {
            get { return "@MP_URL_FAILURE";}
        }

        public static string MP_REGISTER_SB
        {
            get { return "@MP_REGISTER_SB";}
        }

        public static string MP_PAYMENTDATETIME
        {
            get {return "@MP_PAYMENTDATETIME";}
        }

        public static string REQUEST_PAYMENT_ID
        {
            get {return "@REQUEST_PAYMENT_ID";}
        }

        public static string SP_CREATE_REQUEST_ENTERPRISE_PAYMENT
        {
            get {return "SP_CREATE_REQUEST_ENTERPRISE_PAYMENT";}
        }

        public static string SP_EP_GET_REQUESTPAYMENT_BY_ID
        {
            get {return "SP_EP_GET_REQUESTPAYMENT_BY_ID";}
        } 

        public static string REQUEST_PAYMENT_ID_OUTPUT_SEARCH
        {
            get {return "REQUEST_PAYMENT_ID";}
        }

        public static string DATETIMEFORMAT 
        {
            get {return "yyyy-MM-dd HH:mm:ss.fff";}
        }

        public static string LogTemplateBeforeInsert 
        {
            get {
                return "Before Insert Request Payment with the following data:"+
                "MpAccount: {MpAccount}"+
                "MpProduct: {MpProduct}"+
                "MpOrder:{MpOrder}"+
                "MpReference:{MpReference}"+
                "MpNode:{MpNode}"+
                "MpConcept:{MpConcept}"+
                "MpAmount:{MpAmount}"+
                "MpCustomerName:{MpCustomerName}"+
                "MpCurrency:{MpCurrency}"+
                "MpSignature:{MpSignature}"+
                "MpUrlSuccess:{MpUrlSuccess}"+
                "MpUrlFailure:{MpUrlFailure}"+
                "MpRegisterSb:{MpRegisterSb}"+
                "MpPaymentDateTime:{MpPaymentDateTime}" + 
                "PaymentStage: {PaymentStage}"+ 
                "ComunicationStep:{ComunicationStep}";
                }
        }
        
        public static string PaymentStage 
        {
            get {return "RequestPayment";}
        }

        public static string ComunicationStep 
        {
            get {return "FROM .NET TO MULTIPAGOS BANCOMER";}
        }

        public static string LogTemplateAfterInsert
        {
            get
            {

                return "After Insert Request Payment with the following data:" +
                        "Generated Id for Request Payment {RequestPaymentGeneratedId}" +
                        "from the ServiceRequest: {ServiceRequest}" +
                        "and BillingAccount: {BillingAccount}";
            }
        }

    }
}