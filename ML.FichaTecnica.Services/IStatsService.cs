using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ML.FichaTecnica.BusinessEntities;

namespace ML.FichaTecnica.Services
{
    public interface IStatsService
    {
        Task RecordEvent(string eventName, string id);
        Task<int> CallsByIdCount(string id);

        Task<IList<EventRecordDto>> ExtractStatsById(string id);
        Task<IList<EventRecordDto>> ExtractStatsByDateRange(DateTimeOffset dateFrom, DateTimeOffset dateTo);
    }
}