using EPWebAPI.Interfaces;
using EPWebAPI.Models;
using Microsoft.Extensions.DependencyInjection;

namespace EPWebAPI.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection RegisterServices(
    this IServiceCollection services)
        {
            services.AddTransient<IDbConnectionRepository, DbConnectionRepository>();
            services.AddTransient<IDbLoggerRepository, DbLoggerRepository>();
            services.AddTransient<IRequestPaymentRepository, RequestPaymentRepository>();
            services.AddTransient<ILogPaymentRepository, LogPaymentRepository>();
            services.AddTransient<IEndPaymentRepository, EndPaymentRepository>();
            services.AddTransient<IResponseBankRequestTypeTibcoRepository, ResponseBankRequestTypeTibcoRepository>();
            services.AddTransient<IHashRepository, HashRepository>();
            services.AddTransient<IResponsePaymentRepository, ResponsePaymentRepository>();
            services.AddTransient<ISentToTibcoRepository, SentToTibcoRepository>();

            return services;
        }
    }
}
