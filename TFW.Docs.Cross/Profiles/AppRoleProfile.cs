﻿using AutoMapper;
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
            CreateMap<AppRoleEntity, RoleSimpleModel>();

            CreateMap<AppRoleEntity, RoleBaseModel>();
            
            CreateMap<AppRoleEntity, RoleDetailModel>();
            
            CreateMap<AppRoleEntity, ListRoleResponseModel>();
        }
    }
}
