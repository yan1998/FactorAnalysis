using AutoMapper;
using BusinessLogic.Models;
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
