using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using TFW.Cross.Entities;

namespace TFW.Cross.Profiles
{
    public class NoteProfile : Profile
    {
        public NoteProfile()
        {
            CreateMap<Note, Models.AppUser.NoteResponseModel>();
        }
    }
}
