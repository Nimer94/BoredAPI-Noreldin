using BoredApi.Service.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(BoredApi.Service.Startup))]
namespace BoredApi.Service
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddHttpClient();

            //builder.Services.AddTransient<IActibityTableRepository, ActibityTableRepository>();
            builder.Services.AddScoped<IBoredAPIManagerService, BoredAPIManagerService>();
        }
    }
}
