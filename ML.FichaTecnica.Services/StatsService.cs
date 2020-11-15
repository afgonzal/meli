using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using ML.FichaTecnica.BusinessEntities;
using ML.FichaTecnica.Stats.DataLayer;

namespace ML.FichaTecnica.Services
{
    public class StatsService : IStatsService
    {
        private readonly IStatsDataAccess _dataAccess;
        private readonly ILogger<StatsService> _logger;
        private readonly IMapper _mapper;

        public StatsService(IStatsDataAccess dataAccess, ILogger<StatsService> logger, IMapper mapper)
        {
            _dataAccess = dataAccess;
            _logger = logger;
            _mapper = mapper;
        }

        public Task RecordEvent(string eventName, string id)
        {
            _logger.LogInformation("Event {eventName}-{id} took {ms}ms.", eventName, id);
            return Task.CompletedTask;
        }

        public async Task<int> CallsByIdCount(string id)
        {
            await _dataAccess.ExtractStatsById(id);
            return 0;
        }

        public async Task<IList<EventRecordDto>> ExtractStatsById(string id)
        {
            var stats = await _dataAccess.ExtractStatsById(id);
            return _mapper.Map<IList<EventRecordDto>>(stats);
        }

        public async Task<IList<EventRecordDto>> ExtractStatsByDateRange(DateTimeOffset dateFrom, DateTimeOffset dateTo)
        {
            var stats = await _dataAccess.ExtractStatsByDateRange(dateFrom, dateTo);
            return _mapper.Map<IList<EventRecordDto>>(stats);
        }
    }
}
