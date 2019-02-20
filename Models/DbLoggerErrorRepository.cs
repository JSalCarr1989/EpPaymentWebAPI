using System;
using EPWebAPI.Interfaces;
using Serilog;

namespace EPWebAPI.Models
{
    public class DbLoggerErrorRepository : IDbLoggerErrorRepository
    {

        
        private readonly ILogger _logger;
        private readonly IEnvironmentSettingsRepository _environmentSettingsRepository;

        public DbLoggerErrorRepository(
                                       IDbConnectionRepository connectionStringRepo, 
                                       IEnvironmentSettingsRepository environmentSettingsRepository
                                       )
        {
           
            _environmentSettingsRepository = environmentSettingsRepository;

            var logger = new LoggerConfiguration()
               .MinimumLevel.Information()
               .WriteTo.MSSqlServer(_environmentSettingsRepository.GetConnectionString(), EpLogErrorTable)
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

        public static string LogCompute256HashMessageTemplateError => @"The error: {@error} has ocurred in Compute256Hash function";

        public static string LogByteToStringMessageTemplateError => @"The error: {@error} has ocurred in ByteToString function";

        public static string LogCreateRequestHashMessageTemplateError => @"The error: {@error} has ocurred in CreateRequestHash function";

        public static string LogCreateRequestPaymentMessageTemplateError => @"The error: {@error} has ocurred in CreateRequestPayment function 
                                                                               for the MpReference:{@mpReference} and MpOrder:{@mpOrder}"; 

        public void LogByteToStringError(string error)
        {
            _logger.Error(LogByteToStringMessageTemplateError, error);
        }

        public void LogCompute256HashError(string error)
        {
            _logger.Error(LogCompute256HashMessageTemplateError, error);
        }

        public void LogCreateRequestHashError(string error)
        {
            _logger.Error(LogCreateRequestHashMessageTemplateError, error);
        }

        public void LogCreateRequestPaymentError(string error, string mpReference, string mpOrder)
        {
            _logger.Error(LogCreateRequestPaymentMessageTemplateError, error, mpReference,mpOrder);
        }

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
