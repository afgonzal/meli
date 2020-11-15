using System.Collections.Generic;

namespace ML.FichaTecnica.BusinessEntities
{
    /// <summary>
    /// Salida del merge de FT y atributos de un Item
    /// </summary>
    public class ItemAttributesOutput
    {
        /// <summary>
        /// Item Id (de ML)
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Desc del articulo
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Groups de la FT
        /// </summary>
        public IList<GroupOutput> Groups { get; set; }
    }
}