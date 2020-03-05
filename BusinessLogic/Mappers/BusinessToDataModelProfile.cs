using AutoMapper;
using DataAccess.Model;
using System.Collections.Generic;

namespace BusinessLogic.Mappers
{
    public class BusinessToDataModelProfile : Profile
    {
        public BusinessToDataModelProfile()
        {
            CreateMap<DomainModel.ExchangeRateFactors.ExchangeRateFactors, ExchangeRateFactors>();

            CreateMap<List<DomainModel.ForecastingTasks.ForecastingTaskFactorDeclaration>, ForecastingTaskDeclaration>()
                .ForMember(x => x.FactorsDeclaration, opt => opt.MapFrom(y => y))
                .ForMember(x => x.Name, opt => opt.Ignore())
                .ForMember(x => x.Description, opt => opt.Ignore())
                .ForMember(x => x._id, opt => opt.Ignore());

            CreateMap<DomainModel.ForecastingTasks.ForecastingTaskFactorDeclaration, ForecastingTaskFactorDeclaration>();
            CreateMap<DomainModel.ForecastingTasks.ForecastingTaskFactorValue, ForecastingTaskFactorValue>();
        }
    }
}
