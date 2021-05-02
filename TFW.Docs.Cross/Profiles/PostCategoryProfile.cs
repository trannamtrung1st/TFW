﻿using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using TFW.Docs.Cross.Entities;
using TFW.Docs.Cross.Models.PostCategory;

namespace TFW.Docs.Cross.Profiles
{
    public class PostCategoryProfile : Profile
    {
        public PostCategoryProfile()
        {
            CreateMap<PostCategoryLocalizationEditableModel, PostCategoryLocalization>();

            CreateMap<CreatePostCategoryModel, PostCategory>();
        }
    }
}
