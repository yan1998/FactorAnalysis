﻿using AutoMapper;
using DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic.Mappers
{
    public class DataModelToBusinessProfile : Profile
    {
        public DataModelToBusinessProfile()
        {
            CreateMap<ExchangeRateFactors, BusinessLogic.Models.ExchangeRateFactors>();
        }
    }
}
