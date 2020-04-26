using AutoMapper;
using DataAccess.Model;

namespace BusinessLogic.Mappers
{
    public class DataModelToBusinessProfile : Profile
    {
        public DataModelToBusinessProfile()
        {
            CreateMap<ExchangeRateFactors, DomainModel.ExchangeRateFactors.ExchangeRateFactors>();

            CreateMap<ForecastingTaskDeclaration, DomainModel.ForecastingTasks.ShortForecastingTaskInfo>();

            CreateMap<ForecastingTaskFieldDeclaration, DomainModel.ForecastingTasks.ForecastingTaskFieldDeclaration>()
                .ForMember(x => x.Type, opt => opt.MapFrom(y => (DomainModel.ForecastingTasks.FieldType)y.Type));

            CreateMap<ForecastingTaskFieldValue, DomainModel.ForecastingTasks.ForecastingTaskFieldValue>();

            CreateMap<ForecastingTaskFieldValues, DomainModel.ForecastingTasks.ForecastingTaskFieldValues>()
                .ForMember(x => x.Id, opt => opt.MapFrom(y => y._id.ToString()));
        }
    }
}
