using AutoMapper;
using System;
using System.Globalization;
using System.Security.Claims;
using TFW.Docs.Cross.Models.Common;

namespace TFW.Docs.Cross.Profiles
{
    public class CommonModelsProfile : Profile
    {
        public CommonModelsProfile()
        {
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
