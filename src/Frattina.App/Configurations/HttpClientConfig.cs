using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;

namespace Frattina.App.Configurations
{
    public static class HttpClientConfig
    {
        public static IServiceCollection AddHttpClientConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient("sap", c =>
            {
                c.BaseAddress = new Uri(configuration.GetSection("SapBaseAddress").Value);
            })
                .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
                {
                    ClientCertificateOptions = ClientCertificateOption.Manual,
                    ServerCertificateCustomValidationCallback =
                        (httpRequestMessage, cert, cetChain, SslPolicyErrors) =>
                        {
                            return true;
                        }
                });

            return services;
        }
    }
}
