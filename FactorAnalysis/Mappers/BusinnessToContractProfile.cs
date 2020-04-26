﻿using AutoMapper;
using DomainModel.ForecastingTasks;
using FactorAnalysis.Model.Responses;

namespace FactorAnalysis.Mappers
{
    public class BusinnessToContractProfile : Profile
    {
        public BusinnessToContractProfile()
        {
            CreateMap<DomainModel.ForecastingTasks.ShortForecastingTaskInfo, GetForecastingTaskEntitiesResponse>();

            CreateMap<PagedForecastingTask, GetPagedForecastingTaskResponse>();
            CreateMap<DomainModel.ForecastingTasks.ForecastingTaskFieldDeclaration, Model.ForecastingTaskFieldDeclaration>();
            CreateMap<DomainModel.ForecastingTasks.ForecastingTaskFieldValues, Model.ForecastingTaskFieldValues>();
            CreateMap<DomainModel.ForecastingTasks.ForecastingTaskFieldValue, Model.ForecastingTaskFieldValue>();
        }
    }
}
