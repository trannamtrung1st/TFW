using AutoMapper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Claims;
using System.Text;
using TFW.Docs.Cross.Models.Common;

namespace TFW.Docs.Cross.Profiles
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

            CreateMap<TimeZoneInfo, TimeZoneOption>()
                .ForMember(o => o.UtcOffsetInMinutes, opt => opt.MapFrom(o => o.BaseUtcOffset.TotalMinutes));

            CreateMap<CultureInfo, CultureOption>();

            CreateMap<RegionInfo, CurrencyOption>();

            CreateMap<RegionInfo, RegionOption>();
        }
    }
}
