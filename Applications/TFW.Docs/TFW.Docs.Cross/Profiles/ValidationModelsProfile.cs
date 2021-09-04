using AutoMapper;
using FluentValidation.Results;
using TFW.Docs.Cross.Models.Common;

namespace TFW.Docs.Cross.Profiles
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
