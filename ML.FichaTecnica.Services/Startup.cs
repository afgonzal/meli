using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ML.FichaTecnica.Services
{
    public static class Startup
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration config)
        {
            services.AddHttpClient<IMeliClient, MeliClientService>(client =>
            {
                client.BaseAddress = new Uri(config["MLApiBaseUrl"]);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            }).SetHandlerLifetime(TimeSpan.FromMinutes(10));

            

            services.AddSingleton<INumbersToLanguage, NumerosLeiblesService>();
            //services.AddScoped<IMeliClient, MeliClientService>();
            services.AddScoped<IFichaTecnicaService, FichaTecnicaService>();
        }
    }
}
