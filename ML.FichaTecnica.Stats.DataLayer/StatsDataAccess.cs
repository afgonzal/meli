using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace ML.FichaTecnica.Stats.DataLayer
{
    /// <summary>
    /// Maneja Acceso a archivo plano (.log generado por nlog) que contiene las Stats
    /// </summary>
    public class StatsDataAccess : IStatsDataAccess
    {
        private readonly string _sourceFileName;
        private readonly string _separator;

        public StatsDataAccess(IConfiguration config, string enviromentPath)
        {
            _sourceFileName = Path.Combine(enviromentPath, config["StatsFile"]);
            _separator = config["StatsSeparator"];
        }
        public Task<IList<EventRecord>> ExtractStatsById(string id)
        {
            return FilterStats(values => values[3] == id);
        }

        public Task<IList<EventRecord>> ExtractStatsByDateRange(DateTimeOffset dateFrom, DateTimeOffset dateTo)
        {
            dateTo = dateTo.AddDays(1);
            return FilterStats(values =>
            {
                var statDate = DateTimeOffset.Parse(values[0]);
                return statDate >= dateFrom && statDate < dateTo;
            });
        }

   

        private async Task<IList<EventRecord>> FilterStats(Func<string[], bool> filter)
        {
            var idLines = new List<EventRecord>();
            using (var sr = File.OpenText(_sourceFileName))
            {
                string line;
                var lineCount = 1;
                while ((line = await sr.ReadLineAsync()) != null)
                {
                    lineCount++;
                    var values = line.Split(_separator, StringSplitOptions.None);
                    if (values.Length != 4)
                    {
                        throw new ArgumentException($"Invalid status line, 5 columns expected.Line: {line}");
                    }
                    if (filter(values))
                    {
                        idLines.Add(new EventRecord { Id = values[3], EventName = values[2], Message = values[1], DateTime = DateTimeOffset.Parse(values[0]) });
                    }
                }
            }

            return idLines;
        }
    }
}
