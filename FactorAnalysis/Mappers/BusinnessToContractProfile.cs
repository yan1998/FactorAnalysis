using AutoMapper;
using DomainModel.ForecastingTasks;
using FactorAnalysis.Model.Responses;

namespace FactorAnalysis.Mappers
{
    public class BusinnessToContractProfile : Profile
    {
        public BusinnessToContractProfile()
        {
            CreateMap<DomainModel.ForecastingTasks.ShortForecastingTaskInfo, GetForecastingTaskEntitiesResponse>();

            CreateMap<PagedForecastingTask, GetPagedForecastingTaskResponse>()
                .ForMember(x => x.FieldsValues, opt => opt.MapFrom(y => y.Records));
            CreateMap<DomainModel.ForecastingTasks.ForecastingTaskFieldDeclaration, Model.ForecastingTaskFieldDeclaration>();
            CreateMap<DomainModel.ForecastingTasks.ForecastingTaskRecord, Model.ForecastingTaskFieldValues>();
            CreateMap<DomainModel.ForecastingTasks.ForecastingTaskFieldValue, Model.ForecastingTaskFieldValue>();

            CreateMap<DomainModel.ForecastingTasks.AlgorithmPredictionReport, Model.AnalyzePredictionAlgorithmsReport>();
            CreateMap<DomainModel.ForecastingTasks.AlgorithmPredictionResult, Model.AlgorithmPredictionResult>();
        }
    }
}
