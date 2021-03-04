using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using TFW.Cross.Entities;
using TFW.Cross.Models.AppRole;

namespace TFW.Cross.Profiles
{
    public class AppRoleProfile : Profile
    {
        public AppRoleProfile()
        {
            CreateMap<AppRole, GetListRolesResponseModel>();
        }
    }
}
