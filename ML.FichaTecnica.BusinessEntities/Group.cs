using System.Collections.Generic;
using System.Dynamic;

namespace ML.FichaTecnica.BusinessEntities
{
    public class Group
    {
        public string Id { get; set; }
        public string Label { get; set; }
        public string Section { get; set; }
        public IList<Component> Components { get; set; }
    }
}