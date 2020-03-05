using AutoMapper;
using DataAccess.Model;

namespace BusinessLogic.Mappers
{
    public class DataModelToBusinessProfile : Profile
    {
        public DataModelToBusinessProfile()
        {
            CreateMap<ExchangeRateFactors, DomainModel.ExchangeRateFactors.ExchangeRateFactors>();
            CreateMap<ForecastingTaskFactorDeclaration, DomainModel.ForecastingTasks.ForecastingTaskFactorDeclaration>();
            CreateMap<ForecastingTaskFactorValue, DomainModel.ForecastingTasks.ForecastingTaskFactorValue>();
            CreateMap<ForecastingTaskFactorValues, DomainModel.ForecastingTasks.ForecastingTaskFactorValues>()
                .ForMember(x => x.id, opt => opt.MapFrom(y => y._id.ToString()));
        }
    }
}
