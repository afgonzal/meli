using System;
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
        Task<ItemAttributesOutput> BuildItemAttributes(string itemId);
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

        public async Task<ItemAttributesOutput> BuildItemAttributes(string itemId)
        {
            _logger.LogDebug($"BuildItemAttributes start item:{itemId}");
            
            var item = await _backend.GetItem(itemId);
            
            var result = new ItemAttributesOutput {Id = itemId, Title =  item.Title, Groups = new List<GroupOutput>()};

            var techSpecs = await _backend.GetTechnicalSpecs(item.DomainId);

            var sw = new Stopwatch();
            sw.Start();
            foreach (var group in techSpecs.Groups)
            {
                var grpOutput = new GroupOutput {Label = group.Label, Components = new List<ComponentOutput>()};
                foreach (var component in group.Components)
                {
                    if (Enum.TryParse<ComponentTypes>(component.ComponentType, true, out ComponentTypes componentType))
                    {
                        foreach (var attribute in component.Attributes)
                        {

                            var itemAttribute = item.Attributes.SingleOrDefault(attr => attr.Id == attribute.Id);
                            if (itemAttribute != null)
                            {
                                _logger.LogDebug($"Attribute {itemAttribute.Id} found. t:{sw.ElapsedMilliseconds}ms");

                                grpOutput.Components.Add(new ComponentOutput
                                {
                                    Id = itemAttribute.Id, Name = itemAttribute.Name, Value = itemAttribute.ValueName,
                                    ComponentType = componentType
                                });
                                var t = itemAttribute.ValueName;
                            }
                        }
                    }
                }
                if (grpOutput.Components.Any())
                    result.Groups.Add((grpOutput));
            }
            sw.Stop();
            _logger.LogDebug($"BuildItemAttributes end item:{itemId}, time:{sw.ElapsedMilliseconds}ms");

            return result;
        }

        private T ReadProperty<T>(JObject json, string propertyName)
        {
            return default(T);
        }

       
    }
}
