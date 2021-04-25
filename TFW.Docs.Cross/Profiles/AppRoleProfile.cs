using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using TFW.Docs.Cross.Entities;
using TFW.Docs.Cross.Models.AppRole;

namespace TFW.Docs.Cross.Profiles
{
    public class AppRoleProfile : Profile
    {
        public AppRoleProfile()
        {
            CreateMap<AppRole, RoleSimpleModel>();

            CreateMap<AppRole, RoleBaseModel>();
            
            CreateMap<AppRole, RoleDetailModel>();
            
            CreateMap<AppRole, GetListRolesResponseModel>();
        }
    }
}
