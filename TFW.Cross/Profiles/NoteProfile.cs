using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using TFW.Cross.Entities;
using TFW.Cross.Models.Note;

namespace TFW.Cross.Profiles
{
    public class NoteProfile : Profile
    {
        public NoteProfile()
        {
            CreateMap<Note, NoteSimpleModel>();

            CreateMap<Note, NoteBaseModel>();

            CreateMap<Note, NoteDetailModel>();
        }
    }
}
