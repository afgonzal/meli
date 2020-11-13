using System.Threading.Tasks;
using ML.FichaTecnica.BusinessEntities;
using Newtonsoft.Json.Linq;

namespace ML.FichaTecnica.Services
{
    /// <summary>
    /// Integración con backend de ML 
    /// </summary>
    public interface IMeliClient
    {
        /// <summary>
        /// Trae item by Id
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        Task<Item> GetItem(string itemId);
        /// <summary>
        /// Trae Ficha Técnica para un dominio ej.Celulares
        /// </summary>
        /// <param name="domain"></param>
        /// <returns></returns>
        Task<TechnicalSpecs> GetTechnicalSpecs(string domain);
    }
}