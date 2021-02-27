using AutoMapper;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Text;
using TFW.Cross.Models.Common;

namespace TFW.Cross.Profiles
{
    public class ValidationModelsProfile : Profile
    {
        public ValidationModelsProfile()
        {
            CreateMap<ValidationFailure, AppResult>()
                .ForMember(o => o.Code, opt => opt.MapFrom(o => o.CustomState))
                .ForMember(o => o.Message, opt => opt.MapFrom(o => o.ErrorMessage));
        }
    }
}
