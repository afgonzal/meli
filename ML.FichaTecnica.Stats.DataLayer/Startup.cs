using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ML.FichaTecnica.Stats.DataLayer
{
    public static class Startup
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration config, string runningPath)
        {
            services.AddTransient<IStatsDataAccess>(opt =>
                ActivatorUtilities.CreateInstance<StatsDataAccess>(opt, runningPath)
            );
        }
    }
}
