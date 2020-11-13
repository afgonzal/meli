using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace ML.FichaTecnica.Services
{
    /// <summary>
    /// REST Client para consumir API pública de ML
    /// </summary>
    public class MeliClientService : IMeliClient
    {
        private readonly HttpClient _client;

        public MeliClientService(IConfiguration config)
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _client.BaseAddress = new Uri(config["MLApiBaseUrl"]);
        }

        public async Task<JObject> GetItem(string itemId)
        {
            var response = await _client.GetStringAsync($"items/{itemId}");

            return JObject.Parse(response);
        }

        public async Task<JObject> GetTechnicalSpecs(string domain)
        {
            var response = await _client.GetStringAsync($"domains/{domain}/technical_specs/output");

            return JObject.Parse(response);
        }
    }
}
