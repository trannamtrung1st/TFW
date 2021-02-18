using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using TFW.Cross.Entities;
using TFW.Cross.Models.AppUser;

namespace TFW.Cross.Profiles
{
    public class AppUserProfile : Profile
    {
        public AppUserProfile()
        {
            CreateMap<AppUser, AppUserResponseModel>();

            CreateMap<GetAppUserListRequestModel, DynamicQueryAppUserModel>();
            // Not necessary because of AutoMapper case insensitive
            //.ForMember(o => o.UserName, opt => opt.MapFrom(o => o.username))
            //.ForMember(o => o.Id, opt => opt.MapFrom(o => o.id));
        }
    }
}
