using Newtonsoft.Json;
using System.Collections.Generic;

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

    public class TechnicalSpecs
    {
        [JsonProperty("main_title")]
        public string MainTitle { get; set; }

        public IList<Group> Groups { get; set; }
    }
    public class Attribute
    {
        public string Id { get; set; }
        [JsonProperty("value_name")]
        public string ValueName { get; set; }

        public string Name { get; set; }
    }

    public class Group
    {
        public string Id { get; set; }
        public string Label { get; set; }
        public string Section { get; set; }
        public IList<Component> Components { get; set; }
    }

    public class Component
    {
        [JsonProperty("component")]
        public string ComponentType { get; set; }
        public string Label { get; set; }

        public IList<Attribute> Attributes { get; set; }
    }
}
