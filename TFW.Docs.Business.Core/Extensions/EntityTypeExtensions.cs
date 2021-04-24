using System;
using System.Collections.Generic;
using System.Text;
using TFW.Docs.Cross.Entities;

namespace TFW.Docs.Business.Core.Extensions
{
    public static class EntityTypeExtensions
    {
        public static bool IsAppUserEntity(this Type type)
        {
            return typeof(AppUser).IsAssignableFrom(type);
        }
    }
}
