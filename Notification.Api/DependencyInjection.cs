using Asp.Versioning;
using Notification.Api.Infrastructure;
using Serilog;

namespace Notification.Api
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApiVersion(this IServiceCollection services)
        {
            services.AddApiVersioning(setup =>
            {
                setup.DefaultApiVersion = new ApiVersion(1, 0);
                setup.AssumeDefaultVersionWhenUnspecified = true;
                setup.ReportApiVersions = false;
                setup.ApiVersionReader = new UrlSegmentApiVersionReader();
            });

            return services;
        }

        public static IServiceCollection AddMiddleware(this IServiceCollection services) 
        {
            services.AddTransient<ExceptionHandlingMiddleware>();

            return services;
        }

        public static IServiceCollection AddSerilogger(this IServiceCollection services, IConfiguration configuration)
        {
            var logger = new LoggerConfiguration()
                       .ReadFrom.Configuration(configuration)
                       .CreateLogger();

            services.AddSerilog(logger);
            services.AddHttpLogging((logger) =>
            {
                logger.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.All;
                logger.CombineLogs = true;
            });

            return services;
        }
    }
}
