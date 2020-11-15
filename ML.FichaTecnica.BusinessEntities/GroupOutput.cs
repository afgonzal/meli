using System.Collections.Generic;

namespace ML.FichaTecnica.BusinessEntities
{
    /// <summary>
    /// Groups de la FT (salida mergeada con Item attributes)
    /// </summary>
    public class GroupOutput
    {
        /// <summary>
        /// Nombre del group
        /// </summary>
        public string Label { get; set; }
        /// <summary>
        /// Attributos/Components
        /// </summary>
        public IList<ComponentOutput> Components { get; set; }
    }
}