using AutoMapper;
using DomainModel.ExchangeRateFactors;
using DomainModel.ForecastingTasks;
using FactorAnalysis.Model.Requests;
using FactorAnalysis.Model.Responses;

namespace FactorAnalysis.Mappers
{
    public class ContractToBusinnessProfile : Profile
    {
        public ContractToBusinnessProfile()
        {
            CreateMap<CurrencyExchangePredictionRequest, ExchangeRateFactors>()
                .ForMember(x => x.Date, opt => opt.Ignore())
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ForMember(x => x.ExchangeRateUSD, opt => opt.Ignore())
                .ForMember(x => x.ExchangeRateEUR, opt => opt.Ignore());

            CreateMap<ForecastingTaskFieldDeclarationCreationRequest, DomainModel.ForecastingTasks.ForecastingTaskFieldDeclaration>()
                .ForMember(x => x.Id, opt => opt.Ignore());

            CreateMap<Model.ForecastingTaskFieldValue, DomainModel.ForecastingTasks.ForecastingTaskFieldValue>();

            CreateMap<PagedForecastingTask, GetPagedForecastingTaskResponse>();
            CreateMap<DomainModel.ForecastingTasks.ForecastingTaskFieldDeclaration, Model.ForecastingTaskFieldDeclaration>();
            CreateMap<DomainModel.ForecastingTasks.ForecastingTaskFieldValues, Model.ForecastingTaskFieldValues>();
            CreateMap<DomainModel.ForecastingTasks.ForecastingTaskFieldValue, Model.ForecastingTaskFieldValue>();
        }
    }
}
