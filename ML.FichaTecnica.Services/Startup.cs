using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ML.FichaTecnica.Services
{
    public static class Startup
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration config)
        {
            services.AddSingleton<INumbersToLanguage, NumerosLeiblesService>();
            services.AddScoped<IMeliClient, MeliClientService>();
            services.AddScoped<IFichaTecnicaService, FichaTecnicaService>();
        }
    }
}
