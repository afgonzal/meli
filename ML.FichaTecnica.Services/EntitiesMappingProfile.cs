using AutoMapper;
using ML.FichaTecnica.BusinessEntities;
using ML.FichaTecnica.Stats.DataLayer;

namespace ML.FichaTecnica.Services
{
    public class EntitiesMappingProfile : Profile
    {
        public EntitiesMappingProfile()
        {
            CreateMap<EventRecord, EventRecordDto>();
        }
    }
}
