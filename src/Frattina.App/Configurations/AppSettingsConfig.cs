using Frattina.CrossCutting.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Frattina.App.Configurations
{
    public static class AppSettingsConfig
    {
        public static IServiceCollection AddAppSettingsConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var config = new AppSettings();

            configuration.Bind(config);

            services.AddSingleton(config);

            return services;
        }
    }
}
