using System.Threading.Tasks;
using ML.FichaTecnica.BusinessEntities;

namespace ML.FichaTecnica.Services
{
    public interface IFichaTecnicaService
    {
        Task<ItemAttributesOutput> BuildItemAttributes(string itemId);
    }
}
