using AutoMapper;
using DataAccess.Model;

namespace BusinessLogic.Mappers
{
    public class DataModelToBusinessProfile : Profile
    {
        public DataModelToBusinessProfile()
        {
            CreateMap<ExchangeRateFactors, DomainModel.ExchangeRateFactors.ExchangeRateFactors>();
        }
    }
}
