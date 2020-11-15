using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ML.FichaTecnica.BusinessEntities;
using Newtonsoft.Json;

namespace ML.FichaTecnica.Services
{
    /// <summary>
    /// REST Client para consumir API pública de ML
    /// </summary>
    public class MeliClientService : IMeliClient
    {
        private readonly ILogger<MeliClientService> _logger;
        private readonly HttpClient _client;

        public MeliClientService(ILogger<MeliClientService> logger, HttpClient client)
        {
            _logger = logger;
            _client = client;
        }
        /// <summary>
        /// Trae item by Id
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public async Task<Item> GetItem(string itemId)
        {
            var sw = new Stopwatch();
            sw.Start();
            var response = await _client.GetStreamAsync($"items/{itemId}");
            using (var streamReader = new StreamReader(response))
            using (var reader = new JsonTextReader(streamReader))
            {
                var serializer = new JsonSerializer();
                var result = serializer.Deserialize<Item>(reader);
                sw.Stop();
                _logger.LogDebug($"Item {itemId} back from backend. Elapsed:{sw.ElapsedMilliseconds}ms");
                return result;
            }
           

        }
        /// <summary>
        /// Trae Ficha Técnica para un dominio ej.Celulares
        /// </summary>
        /// <param name="domain"></param>
        /// <returns></returns>
        public async Task<TechnicalSpecs> GetTechnicalSpecs(string domain)
        {
            var sw = new Stopwatch();
            sw.Start();
            var response = await _client.GetStreamAsync($"domains/{domain}/technical_specs/output");

            using (var streamReader = new StreamReader(response))
            using (var reader = new JsonTextReader(streamReader))
            {
                var serializer = new JsonSerializer();
                var result = serializer.Deserialize<TechnicalSpecs>(reader);
                sw.Stop();
                _logger.LogDebug($"TechSpecs {domain} back from backend. Elapsed:{sw.ElapsedMilliseconds}ms");
                return result;
            }
        }
    }
}
