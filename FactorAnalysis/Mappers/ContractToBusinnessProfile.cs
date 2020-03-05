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

            CreateMap<ForecastingTaskFactorDeclarationCreationRequest, DomainModel.ForecastingTasks.ForecastingTaskFactorDeclaration>()
                .ForMember(x => x.Id, opt => opt.Ignore());

            CreateMap<Model.Requests.ForecastingTaskFactorValue, DomainModel.ForecastingTasks.ForecastingTaskFactorValue>();

            CreateMap<PagedForecastingTask, GetPagedForecastingTaskResponse>();
            CreateMap<DomainModel.ForecastingTasks.ForecastingTaskFactorDeclaration, Model.Responses.ForecastingTaskFactorDeclaration>();
            CreateMap<DomainModel.ForecastingTasks.ForecastingTaskFactorValues, Model.Responses.ForecastingTaskFactorValues>();
            CreateMap<DomainModel.ForecastingTasks.ForecastingTaskFactorValue, Model.Requests.ForecastingTaskFactorValue>();
        }
    }
}
