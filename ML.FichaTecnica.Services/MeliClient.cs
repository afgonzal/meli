using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ML.FichaTecnica.BusinessEntities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ML.FichaTecnica.Services
{
    /// <summary>
    /// REST Client para consumir API pública de ML
    /// </summary>
    public class MeliClientService : IMeliClient
    {
        private readonly ILogger<MeliClientService> _logger;
        private readonly HttpClient _client;

        public MeliClientService(IConfiguration config, ILogger<MeliClientService> logger)
        {
            _logger = logger;
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _client.BaseAddress = new Uri(config["MLApiBaseUrl"]);
        }

        public async Task<Item> GetItem(string itemId)
        {
            var response = await _client.GetStreamAsync($"items/{itemId}");
            using (var streamReader = new StreamReader(response))
            using (var reader = new JsonTextReader(streamReader))
            {
                var serializer = new JsonSerializer();

                return serializer.Deserialize<Item>(reader);
            }
        }

        public async Task<TechnicalSpecs> GetTechnicalSpecs(string domain)
        {
            var response = await _client.GetStreamAsync($"domains/{domain}/technical_specs/output");

            using (var streamReader = new StreamReader(response))
            using (var reader = new JsonTextReader(streamReader))
            {
                var serializer = new JsonSerializer();

                return serializer.Deserialize<TechnicalSpecs>(reader);
            }
        }
    }
}
