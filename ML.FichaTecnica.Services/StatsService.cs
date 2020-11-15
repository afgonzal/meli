using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace ML.FichaTecnica.Services
{
    public interface IStatsService
    {
        Task RecordEvent(string eventName, string id);
    }
    public class StatsService : IStatsService
    {
        private readonly ILogger<StatsService> _logger;

        public StatsService(ILogger<StatsService> logger)
        {
            _logger = logger;
        }

        public Task RecordEvent(string eventName, string id)
        {
            _logger.LogInformation("Event {eventName}-{id} took {ms}ms.", eventName, id);
            return Task.CompletedTask;
        }
    }
}
