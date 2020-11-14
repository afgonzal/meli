using System.Collections.Generic;
using Newtonsoft.Json;

namespace ML.FichaTecnica.BusinessEntities
{
    public class Component
    {
        [JsonProperty("component")]
        public string ComponentType { get; set; }
        public string Label { get; set; }

        public IList<ComponentAttribute> Attributes { get; set; }
    }

    public enum ComponentTypes
    {
        NUMBER_OUTPUT, BOOLEAN_OUTPUT, TEXT_OUTPUT, NUMBER_UNIT_OUTPUT, IGNORE
    }
}