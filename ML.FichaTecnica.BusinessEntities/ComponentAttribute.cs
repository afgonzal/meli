using Newtonsoft.Json;

namespace ML.FichaTecnica.BusinessEntities
{
    public class ComponentAttribute
    {
        public string Id { get; set; }
        [JsonProperty("value_name")]
        public string ValueName { get; set; }

        public string Name { get; set; }
        [JsonProperty("value_struct")]
        public ValueStruct ValueStruct { get; set; }
    }
}
