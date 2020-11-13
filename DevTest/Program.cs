using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ML.FichaTecnica.Services;
using NLog.Extensions.Logging;

namespace DevTest
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            //Console.WriteLine("4586 {0}", HumanFriendlyInteger.IntegerToWritten(4586));

            //var numeros = new List<int>
            //{
            //    0, 1, 2, 5, 8, 12, 15, 18, 20, 21,22,23,29, 30, 40, 42, 46, 98, 100, 101,111, 200,222, 300,343,400, 404,411,421,431,441, 444,451,461, 471, 
            //    481, 491,500,508, 600, 666, 700, 745, 800,816,900, 901,915, 999, 1000, 1001, 1010, 1100,
            //    1203, 1220, 1224, 10000, 20000, 20123, 
            //    21312, 45456, 53135,
            //    1000000, 1111111, 1180000, 1180400, 1180430,
            //    1200000, 1250000, 2000000, 12000000, 12345678, -3
            //};
            //numeros.ForEach(x => Console.WriteLine("{0} {1}", x, NumerosLeibles.Int2Espanol(x)));


            //await svc.GetItem("MLA885018383");

            //await svc.GetTechnicalSpecs("MLA-CELLPHONES");


            using (ServiceProvider svcProvider = ConfigureDependencyInjection(LoadConfiguration()))
            {
                var app = svcProvider.GetService<TestMeli>();
                await app.Run();
            }

        }

        public static IConfiguration LoadConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .AddEnvironmentVariables();

            return builder.Build();
        }

        private static ServiceProvider ConfigureDependencyInjection(IConfiguration config)
        {
            var path = Directory.GetCurrentDirectory();


            var services = new ServiceCollection();
            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                loggingBuilder.AddNLog(config);
            });
            services.AddScoped<IConfiguration>(provider => config);


            services.AddTransient<IMeliClient, MeliClientService>();
            services.AddTransient<IFichaTecnicaService, FichaTecnicaService>();


            services.AddTransient<TestMeli>();
            return services.BuildServiceProvider();
        }
    }


    public class TestMeli
    {
        private readonly IFichaTecnicaService _fichaTecnicaService;
        private readonly ILogger<TestMeli> _logger;

        public TestMeli(IFichaTecnicaService fichaTecnicaService, ILogger<TestMeli> logger)
        {
            _fichaTecnicaService = fichaTecnicaService;
            _logger = logger;
        }
        public async Task Run()
        {
            _logger.LogDebug("Arrancando");
            await _fichaTecnicaService.BuildItemAttributes("MLA885018383");
            _logger.LogDebug("Fin corrida App");
        }
    }
    
}
