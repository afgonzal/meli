using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ML.FichaTecnica.BusinessEntities;
using Newtonsoft.Json.Linq;
using Attribute = ML.FichaTecnica.BusinessEntities.Attribute;

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
            //Move List to a Dictionary to improve search efficiency
            var itemAttributes = item.Attributes.ToDictionary(attr => attr.Id, attr => attr);

            var techSpecs = await _backend.GetTechnicalSpecs(item.DomainId);

            var sw = new Stopwatch();
            sw.Start();

            //use threadsafe list to work in parallel
            var resultGroups = new BlockingCollection<GroupOutput>();

            //Split groups in half to work in parallel
            var groups = new IEnumerable<Group>[2];
            groups[0] = techSpecs.Groups.Take(techSpecs.Groups.Count / 2);
            groups[1] = techSpecs.Groups.Skip(techSpecs.Groups.Count / 2);

            Parallel.ForEach(groups, groupSubSet =>
            {
                foreach (var group in groupSubSet)
                {
                    var grpOutput = DigestGroup(group, itemAttributes);
                    //only use groups with content
                    if (grpOutput.Components.Any())
                        resultGroups.Add(grpOutput);
                }
            });
          
    
            sw.Stop();
            _logger.LogDebug($"BuildItemAttributes end item:{itemId}, time:{sw.ElapsedMilliseconds}ms");

            return new ItemAttributesOutput { Id = itemId, Title = item.Title, Groups = resultGroups.ToList()};

        }

        private GroupOutput DigestGroup(Group group, Dictionary<string, Attribute> itemAttributes)
        {
            var grpOutput = new GroupOutput { Label = group.Label, Components = new List<ComponentOutput>() };
            foreach (var component in group.Components)
            {
                if (Enum.TryParse<ComponentTypes>(component.ComponentType, true, out ComponentTypes componentType))
                {
                    foreach (var techAttribute in component.Attributes)
                    {
                        if (itemAttributes.ContainsKey(techAttribute.Id)) //some attributes are not in the Item
                        {
                            var itemAttribute = itemAttributes[techAttribute.Id];

                            grpOutput.Components.Add(new ComponentOutput
                            {
                                Id = itemAttribute.Id,
                                Name = itemAttribute.Name,
                                Value = itemAttribute.ValueName,
                                ComponentType = componentType
                            });
                            var t = itemAttribute.ValueName;
                        }
                    }
                }
            }

            return grpOutput;
        }

        private T ReadProperty<T>(JObject json, string propertyName)
        {
            return default(T);
        }

       
    }
}
