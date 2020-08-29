using KissLog;
using KissLog.Apis.v1.Listeners;
using KissLog.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Frattina.App.Configurations
{
    public static class LogConfig
    {
        public static IServiceCollection AddLogConfiguration(this IServiceCollection services)
        {
            return services.AddScoped(context => Logger.Factory.Get());
        }

        public static IApplicationBuilder UseLog(this IApplicationBuilder app, IConfiguration configuration)
        {
            var organizationId = configuration.GetSection("KissLogOrganizationId").Value;
            var applicationId = configuration.GetSection("KissLogApplicationId").Value;

            return app.UseKissLogMiddleware(options => {
                options.Listeners.Add(new KissLogApiListener(new KissLog.Apis.v1.Auth.Application(organizationId, applicationId)));
            });
        }
    }
}
