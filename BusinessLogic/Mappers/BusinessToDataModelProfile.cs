using AutoMapper;
using DataAccess.Model;
using System.Collections.Generic;

namespace BusinessLogic.Mappers
{
    public class BusinessToDataModelProfile : Profile
    {
        public BusinessToDataModelProfile()
        {
            CreateMap<DomainModel.ExchangeRateFactors.ExchangeRateFactors, ExchangeRateFactors>();

            CreateMap<List<DomainModel.ForecastingTasks.ForecastingTaskFieldDeclaration>, ForecastingTaskDeclaration>()
                .ForMember(x => x.FieldsDeclaration, opt => opt.MapFrom(y => y))
                .ForMember(x => x.Name, opt => opt.Ignore())
                .ForMember(x => x.Description, opt => opt.Ignore())
                .ForMember(x => x._id, opt => opt.Ignore());

            CreateMap<DomainModel.ForecastingTasks.ForecastingTaskFieldDeclaration, ForecastingTaskFieldDeclaration>();
            CreateMap<DomainModel.ForecastingTasks.ForecastingTaskFieldValue, ForecastingTaskFieldValue>();
        }
    }
}
