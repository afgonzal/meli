using System.Collections.Generic;

namespace ML.FichaTecnica.BusinessEntities
{
    public class GroupOutput
    {
        public string Label { get; set; }
        public IList<ComponentOutput> Components { get; set; }
    }
}