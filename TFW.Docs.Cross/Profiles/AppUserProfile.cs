using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using TFW.Docs.Cross.Entities;
using TFW.Docs.Cross.Models.AppUser;

namespace TFW.Docs.Cross.Profiles
{
    public class AppUserProfile : Profile
    {
        public AppUserProfile()
        {
            CreateMap<AppUserEntity, AppUserSimpleModel>();

            CreateMap<AppUserEntity, AppUserBaseModel>();

            CreateMap<AppUserEntity, AppUserDetailModel>();

            CreateMap<AppUserEntity, GetListAppUsersResponseModel>();

            CreateMap<AppUserEntity, UserProfileModel>();

            CreateMap<GetListAppUsersRequestModel, DynamicQueryAppUserModel>();
            // Not necessary because of AutoMapper case insensitive
            //.ForMember(o => o.UserName, opt => opt.MapFrom(o => o.username))
            //.ForMember(o => o.Id, opt => opt.MapFrom(o => o.id));
        }
    }
}
