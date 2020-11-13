using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ML.FichaTecnica.Services
{
    public interface IFichaTecnicaService
    {
        Task BuildItemAttributes(string itemId);
    }

    public class FichaTecnicaService : IFichaTecnicaService
    {
        private readonly IMeliClient _backend;
        private readonly ILogger<FichaTecnicaService> _logger;

        public FichaTecnicaService(IMeliClient client, ILogger<FichaTecnicaService> logger )
        {
            _backend = client;
            _logger = logger;
        }

        public async Task BuildItemAttributes(string itemId)
        {
            _logger.LogDebug($"BuildItemAttributes start item:{itemId}");
            var sw = new Stopwatch();
            sw.Start();
            var item = await _backend.GetItem(itemId);
            
            _logger.LogDebug($"Item back from backend elapsed {sw.ElapsedMilliseconds}ms");

            var itemAttributes = ((JArray)item["attributes"]).ToObject<IList<Attribute>>();

            var domain = item["domain_id"].Value<string>(); // item.Property("domain_id").Value.ToString();

            var techSpecs = await _backend.GetTechnicalSpecs(domain);
            _logger.LogDebug($"TechSpecs from backend elapsed {sw.ElapsedMilliseconds}ms");

            var groups = (JArray) techSpecs["groups"];
            foreach (var group in groups)
            {
                var components = ((JArray) group["components"]).ToObject<IList<Component>>();
                foreach (var component in components)
                {
                    foreach (var attribute in component.Attributes)
                    {
                        var itemAttribute = itemAttributes.SingleOrDefault(attr => attr.Id == attribute.Id);
                        if (itemAttribute != null)
                        {
                            _logger.LogDebug($"Attribute found {sw.ElapsedMilliseconds}ms");

                            var t = itemAttribute.ValueName;
                        }
                    }
                }
            }
            sw.Stop();
            _logger.LogDebug($"BuildItemAttributes end item:{itemId}, time:{sw.ElapsedMilliseconds}ms");

        }

        private T ReadProperty<T>(JObject json, string propertyName)
        {
            return default(T);
        }


        public class Attribute
        {
            public string Id { get; set; }
            [JsonProperty("value_name")]
            public string ValueName { get; set; }

            public string Name { get; set; }
        }

        public class Component
        {
            [JsonProperty("component")]
            public string ComponentType { get; set; }
            public string Label { get; set; }
            
            public IList<Attribute> Attributes { get; set; }
        }
    }
}
