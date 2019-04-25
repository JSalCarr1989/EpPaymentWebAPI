using Microsoft.Extensions.DependencyInjection;
using EPPCIDAL.Interfaces;
using EPPCIDAL.Repositories;
using EPPCIDAL.Services;

namespace EPWebAPI.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection RegisterServices(
    this IServiceCollection services)
        {
            services.AddTransient<IDbConnectionRepository, DbConnectionRepository>();
            services.AddTransient<IDbLoggerConfigurationRepository, DbLoggerConfigurationRepository>();
            services.AddTransient<IDbLoggerRepository, DbLoggerRepository>();
            services.AddTransient<IDbLoggerErrorRepository, DbLoggerErrorRepository>();
            services.AddTransient<IRequestPaymentRepository, RequestPaymentRepository>();
            services.AddTransient<ILogPaymentRepository, LogPaymentRepository>();
            services.AddTransient<IEndPaymentRepository, EndPaymentRepository>();
            services.AddTransient<IHashRepository, HashRepository>();
            services.AddTransient<IResponsePaymentRepository, ResponsePaymentRepository>();  
            services.AddTransient<IEnterprisePaymentMonitorRepository, EnterprisePaymentMonitorRepository>();
            services.AddTransient<IEnvironmentSettingsRepository, EnvironmentSettingsRepository>();
            services.AddTransient<IHashService, HashService>();
            services.AddTransient<ILogPaymentService, LogPaymentService>();
            services.AddTransient<IBeginPaymentRepository, BeginPaymentRepository>();
            services.AddTransient<IDbConsoleLoggerRepository, DbConsoleLoggerRepository>();
            services.AddTransient<IDbConsoleLoggerErrorRepository, DbConsoleLoggerErrorRepository>();
            services.AddTransient<IRequestPaymentService, RequestPaymentService>();
            services.AddTransient<IResponsePaymentService, ResponsePaymentService>();
            services.AddTransient<IEndPaymentService, EndPaymentService>();
            services.AddTransient<IEnvironmentSettingsService, EnvironmentSettingsService>();
            services.AddTransient<IDbConnectionService, DbConnectionService>();

            return services;
        }
    }
}
