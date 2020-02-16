using AutoMapper;

namespace BusinessLogic.Mappers
{
    public static class Extensions
    {
        public static void AddDataModelMappings(this IMapperConfigurationExpression mapperConfigurationExpression)
        {
            mapperConfigurationExpression.AddProfile<BusinessToDataModelProfile>();
            mapperConfigurationExpression.AddProfile<DataModelToBusinessProfile>();
        }
    }
}
