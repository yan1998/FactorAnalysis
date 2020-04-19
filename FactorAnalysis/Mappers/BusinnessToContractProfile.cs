using AutoMapper;
using FactorAnalysis.Model.Responses;
namespace FactorAnalysis.Mappers
{
    public class BusinnessToContractProfile : Profile
    {
        public BusinnessToContractProfile()
        {
            CreateMap<DomainModel.ForecastingTasks.ShortForecastingTaskInfo, GetForecastingTaskEntitiesResponse>();
        }
    }
}
