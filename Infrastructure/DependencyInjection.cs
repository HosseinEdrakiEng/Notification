using Application.Abstraction.IService;
using Application.Common;
using Application.Config;
using Infrastructure.Service;
using Infrastructure.Service.SmsProviders;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddWorker(this IServiceCollection services)
        {
            services.AddHostedService<QueuedHostedService>();
            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<ISmsService, SmsService>();
            services.AddSingleton<IBackgroundTaskQueue>(_ =>
            {
                return new BackgroundTaskQueue(100);
            });

            services.AddKeyedScoped<SmsBaseService, SmsIRService>(SmsProviderType.SmsIR);

            return services;
        }

        public static IServiceCollection AddProviderHttpClient(this IServiceCollection services, IConfiguration configuration)
        {
            var appConfig = configuration.GetSection("SmsIrConfig").Get<SmsIrConfig>();
            services.AddHttpClient("SmsIR", o =>
            {
                o.BaseAddress = new Uri(appConfig.BaseUrl);
                o.Timeout = appConfig.Timeout;
            }).ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
            {
                ClientCertificateOptions = ClientCertificateOption.Manual,
                ServerCertificateCustomValidationCallback = (httpRequestMessage, cert, cetChain, policyErrors) =>
                {
                    return true;
                }
            }).SetHandlerLifetime(TimeSpan.FromMinutes(10));

            return services;
        }

        public static IServiceCollection AddConfigOption(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();
            services.Configure<SmsIrConfig>(configuration.GetSection("SmsIrConfig"));
            return services;
        }
    }
}
