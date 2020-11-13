using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ML.FichaTecnica.BusinessEntities;
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


            var techSpecs = await _backend.GetTechnicalSpecs(item.DomainId);
            _logger.LogDebug($"TechSpecs from backend elapsed {sw.ElapsedMilliseconds}ms");

            foreach (var group in techSpecs.Groups)
            {
                foreach (var component in group.Components)
                {
                    foreach (var attribute in component.Attributes)
                    {
                        var itemAttribute = item.Attributes.SingleOrDefault(attr => attr.Id == attribute.Id);
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

       
    }
}
