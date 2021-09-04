using AutoMapper;
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

            CreateMap<AppUserEntity, ListAppUserModel>();

            CreateMap<AppUserEntity, UserProfileModel>();
        }
    }
}
