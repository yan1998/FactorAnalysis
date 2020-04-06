using AutoMapper;
using DataAccess.Model;

namespace BusinessLogic.Mappers
{
    public class DataModelToBusinessProfile : Profile
    {
        public DataModelToBusinessProfile()
        {
            CreateMap<ExchangeRateFactors, DomainModel.ExchangeRateFactors.ExchangeRateFactors>();
            CreateMap<ForecastingTaskFieldDeclaration, DomainModel.ForecastingTasks.ForecastingTaskFieldDeclaration>();
            CreateMap<ForecastingTaskFieldValue, DomainModel.ForecastingTasks.ForecastingTaskFieldValue>();
            CreateMap<ForecastingTaskFieldValues, DomainModel.ForecastingTasks.ForecastingTaskFieldValues>()
                .ForMember(x => x.id, opt => opt.MapFrom(y => y._id.ToString()));
        }
    }
}
