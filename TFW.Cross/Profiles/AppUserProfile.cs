using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using TFW.Cross.Entities;
using TFW.Cross.Models;

namespace TFW.Cross.Profiles
{
    public class AppUserProfile : Profile
    {
        public AppUserProfile()
        {
            CreateMap<AppUser, AppUserResponseModel>();
        }
    }
}
