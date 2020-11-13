using System.Collections.Generic;
using Newtonsoft.Json;

namespace ML.FichaTecnica.BusinessEntities
{
    public class Item
    {
        public string Id { get; set; }
        public string Title { get; set; }
        [JsonProperty("category_id")]
        public string CategoryId { get; set; }

        public IList<Attribute> Attributes { get; set; }

        [JsonProperty("domain_id")]
        public string DomainId { get; set; }
    }
}