using System.Collections.Generic;
using Newtonsoft.Json;

namespace ML.FichaTecnica.BusinessEntities
{
    public class TechnicalSpecs
    {
        [JsonProperty("main_title")]
        public string MainTitle { get; set; }

        public IList<Group> Groups { get; set; }
    }
}