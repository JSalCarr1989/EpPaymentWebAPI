using System.Security.Cryptography;

namespace EPWebAPI.Utilities 
{
    public static class StaticRequestEP 
    {
        public static string MP_ACCOUNT => "@MP_ACCOUNT";

        public static string MP_PRODUCT => "@MP_PRODUCT";

        public static string MP_ORDER => "@MP_ORDER";

        public static string MP_REFERENCE => "@MP_REFERENCE";

        public static string MP_NODE => "@MP_NODE";

        public static string MP_CONCEPT => "@MP_CONCEPT";

        public static string MP_AMOUNT => "@MP_AMOUNT";

        public static string MP_CUSTOMER_NAME => "@MP_CUSTOMER_NAME";

        public static string MP_CURRENCY => "@MP_CURRENCY";

        public static string MP_SIGNATURE => "@MP_SIGNATURE";

        public static string MP_URL_SUCCESS => "@MP_URL_SUCCESS";

        public static string MP_URL_FAILURE => "@MP_URL_FAILURE";

        public static string MP_REGISTER_SB => "@MP_REGISTER_SB";

        public static string MP_PAYMENTDATETIME => "@MP_PAYMENTDATETIME";

        public static string REQUEST_PAYMENT_ID => "@REQUEST_PAYMENT_ID";

        public static string BEGIN_PAYMENT_ID => "@BEGIN_PAYMENT_ID";

        public static string SP_CREATE_REQUEST_ENTERPRISE_PAYMENT => "SP_CREATE_REQUEST_ENTERPRISE_PAYMENT";

        public static string SP_EP_GET_REQUESTPAYMENT_BY_ID => "SP_EP_GET_REQUESTPAYMENT_BY_ID";

        public static string REQUEST_PAYMENT_ID_OUTPUT_SEARCH => "REQUEST_PAYMENT_ID";

        public static string DATETIMEFORMAT => "yyyy-MM-dd HH:mm:ss.fff";

        public static string LogTemplateBeforeInsert => "Before Insert Request Payment with the following data:" +
                "MpAccount: {MpAccount}" +
                "MpProduct: {MpProduct}" +
                "MpOrder:{MpOrder}" +
                "MpReference:{MpReference}" +
                "MpNode:{MpNode}" +
                "MpConcept:{MpConcept}" +
                "MpAmount:{MpAmount}" +
                "MpCustomerName:{MpCustomerName}" +
                "MpCurrency:{MpCurrency}" +
                "MpSignature:{MpSignature}" +
                "MpUrlSuccess:{MpUrlSuccess}" +
                "MpUrlFailure:{MpUrlFailure}" +
                "MpRegisterSb:{MpRegisterSb}" +
                "MpPaymentDateTime:{MpPaymentDateTime}" +
                "PaymentStage: {PaymentStage}" +
                "ComunicationStep:{ComunicationStep}";

        public static string PaymentStage => "RequestPayment";

        public static string ComunicationStep => "FROM .NET TO MULTIPAGOS BANCOMER";

        public static string LogTemplateAfterInsert => "After Insert Request Payment with the following data:" +
                        "Generated Id for Request Payment {RequestPaymentGeneratedId}" +
                        "from the ServiceRequest: {ServiceRequest}" +
                        "and BillingAccount: {BillingAccount}";

        public static string ComputeSha256Hash(string rawData,string secret)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                secret = secret ?? "";

                var encoding = new System.Text.ASCIIEncoding();

                 

                byte[] keyByte = System.Text.Encoding.UTF8.GetBytes(secret);

                byte[] messageBytes = System.Text.Encoding.UTF8.GetBytes(rawData);

                using (var hmacsha256 = new HMACSHA256(keyByte))
                {
                    byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
                    return ByteToString(hashmessage);
                }
            }
        }

        public static string ByteToString(byte[] buff)
        {
            string sbinary = "";


            for (int i = 0; i < buff.Length; i++)
            {
                sbinary += buff[i].ToString("X2"); // hex format
            }
            return (sbinary).ToLower();
        }
    }
}