using AutoMapper;
using DomainModel.ExchangeRateFactors;
using FactorAnalysis.Model.Requests;

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
        }
    }
}
