using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ML.FichaTecnica.Stats.DataLayer
{
    /// <summary>
    /// Obtenter estadisticas de uso
    /// </summary>
    public interface IStatsDataAccess
    {
        /// <summary>
        /// Filtrado por Id (parametro del evento)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IList<EventRecord>> ExtractStatsById(string id);
        /// <summary>
        /// Eventos en un rango de fechas
        /// </summary>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <returns></returns>
        Task<IList<EventRecord>> ExtractStatsByDateRange(DateTimeOffset dateFrom, DateTimeOffset dateTo);
    }
}