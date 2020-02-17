using AutoMapper;
using DomainModel.ExchangeRateFactors;
using FactorAnalysisML.Model.Models;

namespace BusinessLogic.Mappers
{
    public class BusinessToMLModelProfile : Profile
    {
        public BusinessToMLModelProfile()
        {
            CreateMap<ExchangeRateFactors, CurrencyExchangeModelInput>();
        }
    }
}
