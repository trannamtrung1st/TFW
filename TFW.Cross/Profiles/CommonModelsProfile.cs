using AutoMapper;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using TFW.Cross.Models;

namespace TFW.Cross.Profiles
{
    public class CommonModelsProfile : Profile
    {
        public CommonModelsProfile()
        {
            CreateMap<BaseGetListRequestModel, BaseDynamicQueryModel>()
                .ForMember(o => o.Fields, opt => opt.MapFrom(o => o.GetFieldsArr()))
                .ForMember(o => o.SortBy, opt => opt.MapFrom(o => o.GetSortByArr()))
                .IncludeAllDerived();

            CreateMap<ClaimsPrincipal, PrincipalInfo>()
                .ForMember(o => o.UserId, opt => opt.MapFrom(o => o.Identity.Name))
                .ForMember(o => o.IsAuthenticated, opt => opt.MapFrom(o => o.Identity.IsAuthenticated));
        }
    }
}
