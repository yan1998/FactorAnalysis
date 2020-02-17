using AutoMapper;
using DataAccess.Model;

namespace BusinessLogic.Mappers
{
    public class BusinessToDataModelProfile : Profile
    {
        public BusinessToDataModelProfile()
        {
            CreateMap<DomainModel.ExchangeRateFactors.ExchangeRateFactors, ExchangeRateFactors>();
        }
    }
}
