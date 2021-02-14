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
            CreateMap<ClaimsPrincipal, PrincipalInfo>()
                .ForMember(o => o.UserId, opt => opt.MapFrom(o => o.Identity.Name));
        }
    }
}
