using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ML.FichaTecnica.Services;
using ML.FichaTecnica.Stats.DataLayer;
using Newtonsoft.Json;
using NLog.Extensions.Logging;

namespace DevTest
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("AGA testing ML FT");
          
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


            ML.FichaTecnica.Services.Startup.ConfigureServices(services, config, "");

            services.AddTransient<TestMeli>();
            return services.BuildServiceProvider();
        }
    }


    public class TestMeli
    {
        private readonly IFichaTecnicaService _fichaTecnicaService;
        private readonly IStatsService _dataAccess;
        private readonly ILogger<TestMeli> _logger;

        public TestMeli(IFichaTecnicaService fichaTecnicaService, IStatsService statsService, ILogger<TestMeli> logger)
        {
            _fichaTecnicaService = fichaTecnicaService;
            _dataAccess = statsService;
            _logger = logger;
        }
        public async Task Run()
        {
            _logger.LogDebug("Arrancando");
            Console.WriteLine("First run to warmup");
            await _dataAccess.ExtractStatsById("MLA885018383");

            var sw = new Stopwatch();
            sw.Start();
            //var result = await _fichaTecnicaService.BuildItemAttributes("MLA885018383");
            //_logger.LogDebug(JsonConvert.SerializeObject(result));

            var byId = await _dataAccess.ExtractStatsById("MLA885018383");
            Console.WriteLine("ById {0}", byId.Count);
            sw.Stop();

            var byDate =
                await _dataAccess.ExtractStatsByDateRange(new DateTimeOffset(new DateTime(2020, 11, 14), TimeSpan.Zero), new DateTimeOffset(new DateTime(2020, 11, 15), TimeSpan.Zero));
            Console.WriteLine("ByDate {0}", byDate.Count);
            _logger.LogDebug("Fin corrida App {ms}ms", sw.ElapsedMilliseconds);
        }
    }
    
}
