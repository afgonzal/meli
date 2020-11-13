using Newtonsoft.Json;

namespace ML.FichaTecnica.BusinessEntities
{
    public class Attribute
    {
        public string Id { get; set; }
        [JsonProperty("value_name")]
        public string ValueName { get; set; }

        public string Name { get; set; }
    }
}
