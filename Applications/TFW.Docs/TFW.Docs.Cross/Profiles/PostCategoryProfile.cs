using AutoMapper;
using TFW.Docs.Cross.Entities;
using TFW.Docs.Cross.Models.PostCategory;

namespace TFW.Docs.Cross.Profiles
{
    public class PostCategoryProfile : Profile
    {
        public PostCategoryProfile()
        {
            CreateMap<PostCategoryEntity, PostCategoryDetailModel>();

            CreateMap<PostCategoryLocalizationEntity, PostCategoryLocalizationDetailModel>();

            CreateMap<PostCategoryLocalizationEntity, PostCategoryDetailModel>()
                .ForMember(o => o.Id, opt => opt.Ignore())
                .ForMember(o => o.CreatedTime, opt => opt.Ignore())
                .ForMember(o => o.StartingPostId, opt => opt.Ignore());

            CreateMap<PostCategoryLocalizationEditableModel, PostCategoryLocalizationEntity>();

            CreateMap<CreatePostCategoryLocalizationModel, PostCategoryLocalizationEntity>();

            CreateMap<UpdatePostCategoryLocalizationModel, PostCategoryLocalizationEntity>();

            CreateMap<PostCategoryEditableModel, PostCategoryEntity>();

            CreateMap<CreatePostCategoryModel, PostCategoryEntity>();

            CreateMap<UpdatePostCategoryModel, PostCategoryEntity>();
        }
    }
}
