using System.Collections.Generic;

namespace ML.FichaTecnica.BusinessEntities
{
    public class ItemAttributesOutput
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public IList<GroupOutput> Groups { get; set; }
    }
}