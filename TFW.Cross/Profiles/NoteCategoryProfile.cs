using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using TFW.Cross.Entities;
using TFW.Cross.Models.NoteCategory;

namespace TFW.Cross.Profiles
{
    public class NoteCategoryProfile : Profile
    {
        public NoteCategoryProfile()
        {
            CreateMap<NoteCategory, NoteCategorySimpleModel>();

            CreateMap<NoteCategory, NoteCategoryBaseModel>();

            CreateMap<NoteCategory, NoteCategoryDetailModel>();
        }
    }
}
