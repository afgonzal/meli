
namespace ML.FichaTecnica.BusinessEntities
{
    public class ComponentOutput
    {
        /// <summary>
        /// Id del attributo
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Descripcion del attributo
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Valor a mostrar
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// Para los casos de Numer o Number_Unit
        /// </summary>
        public float? Number { get; set; }
        /// <summary>
        /// Unidad (solo para Number_Unit)
        /// </summary>
        public string Unit { get; set; }
    }

   
}