namespace EPWebAPI.Interfaces
{
    public interface IDbLoggerErrorRepository
    {
        void LogSendEndPaymentToTibcoError(string error);
        void LogGetEndPaymentSentToTibcoError(string error);
        void LogValidateMultipagosHashError(string error, string mpReference, string mpOrder);
        void LogGetLastRequestPaymentIdError(string error,string serviceRequest, string paymentReference);
        void LogGenerateResponsePaymentDTOError(string error, string mpReference, string mpOrder);
        void LogCreateResponsePaymentError(string error, string mpReference, string mpOrder);
        void LogGetEndPaymentByResponsePaymentIdError(string error);
        void LogUpdateEndPaymentSentStatusError(string error);
        void LogCompute256HashError(string error);
        void LogByteToStringError(string error);
    }
}
