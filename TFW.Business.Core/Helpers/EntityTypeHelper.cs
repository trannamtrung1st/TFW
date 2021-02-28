using System;
using System.Collections.Generic;
using System.Text;
using TFW.Cross.Entities;

namespace TFW.Business.Core.Helpers
{
    public static class EntityTypeHelper
    {
        public static bool IsNoteEntity(this Type type)
        {
            return typeof(Note).IsAssignableFrom(type);
        }

        public static bool IsAppUserEntity(this Type type)
        {
            return typeof(AppUser).IsAssignableFrom(type);
        }
    }
}
