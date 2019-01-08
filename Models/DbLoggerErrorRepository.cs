using System;
using EPWebAPI.Interfaces;
using Serilog;

namespace EPWebAPI.Models
{
    public class DbLoggerErrorRepository : IDbLoggerErrorRepository
    {

        private readonly IDbConnectionRepository _connectionStringRepo;
        private readonly ILogger _logger;

        public DbLoggerErrorRepository(IDbConnectionRepository connectionStringRepo)
        {
            _connectionStringRepo = connectionStringRepo;

            var logger = new LoggerConfiguration()
               .MinimumLevel.Information()
               .WriteTo.MSSqlServer(_connectionStringRepo.GetEpPaymentConnectionString(), EpLogErrorTable)
               .CreateLogger();

            _logger = logger;
        }

        public static string EpLogErrorTable => "EpLogError";

        public static string ValidateMultipagosHashMessageTemplateError => "The error: {@error} has ocurred for the MpReference:{@mpReference} and MpOrder:{@mpOrder}";

        public static string LogGetLastRequestPaymentIdMessageTemplateError => @"The error: {@error} has ocurred in GetLastRequestPaymentId function 
                                                                                            for the ServiceRequest:{@serviceRequest}
                                                                                            and the PaymentReference:{@paymentReference}";
        public static string LogGenerateResponsePaymentDTOMessageTemplateError => @"The error: {@error} has ocurred in GenerateResponsePaymentDTO function 
                                                                                             for the MpReference:{@mpReference} and MpOrder:{@mpOrder}";

        public static string LogCreateResponsePaymentMessageTemplateError => @"The error: {@error} has ocurred in CreateResponsePayment function
                                                                                          for the MpReference:{@mpReference} and MpOrder: {@mpOrder}";

        public static string LogGetEndPaymentSentToTibcoMessageTemplateError => @"The error: {@error} has ocurred in GetEndPaymentSentToTibco function";

        public static string LogGetEndPaymentByResponsePaymentIdMessageTemplateError => @"The error: {@error} has ocurred in GetEndPaymentByResponsePaymentId function";

        public static string LogSendEndpaymentToTibcoMessageTemplateError => @"The error: {@error} has ocurred in SendEndpaymentToTibco";

        public static string LogUpdateEndPaymentSentStatusMessageTemplateError => @"The error: {@error} has ocurred in UpdateEndPaymentSentStatus function";

        public void LogCreateResponsePaymentError(string error, string mpReference, string mpOrder)
        {
            _logger.Error(LogCreateResponsePaymentMessageTemplateError, error, mpReference, mpOrder);
        }

        public void LogGenerateResponsePaymentDTOError(string error, string mpReference, string mpOrder)
        {
            _logger.Error(LogGenerateResponsePaymentDTOMessageTemplateError, error, mpReference, mpOrder);
        }

        public void LogGetEndPaymentByResponsePaymentIdError(string error)
        {
            _logger.Error(LogGetEndPaymentByResponsePaymentIdMessageTemplateError, error);
        }

        public void LogGetEndPaymentSentToTibcoError(string error)
        {
            _logger.Error(LogGetEndPaymentSentToTibcoMessageTemplateError, error);
        }

        public void LogGetLastRequestPaymentIdError(string error, string serviceRequest, string paymentReference)
        {
            _logger.Error(LogGetLastRequestPaymentIdMessageTemplateError, error, serviceRequest, paymentReference);
        }

        public void LogSendEndPaymentToTibcoError(string error)
        {
            _logger.Error(LogSendEndpaymentToTibcoMessageTemplateError, error);
        }

        public void LogUpdateEndPaymentSentStatusError(string error)
        {
            _logger.Error(LogUpdateEndPaymentSentStatusMessageTemplateError, error);
        }

        public void LogValidateMultipagosHashError(string error, string mpReference, string mpOrder)
        {
            _logger.Error(ValidateMultipagosHashMessageTemplateError, error, mpReference, mpOrder);
        }
    }
}
