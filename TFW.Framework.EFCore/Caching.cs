using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TFW.Framework.Common.Extensions;

namespace TFW.Framework.EFCore
{
    public static class Caching
    {
        public static Type IdentityDbContextType { get; private set; } = typeof(IdentityDbContext);
        public static IEnumerable<string> IdentityEntityTypeNames { get; private set; }

        static Caching()
        {
            var identityEntityTypes = IdentityDbContextType.GetProperties()
                .Where(o => o.PropertyType.GetNameWithoutGenericParameters() ==
                    typeof(DbSet<>).GetNameWithoutGenericParameters())
                .Select(o => o.PropertyType.GetGenericArguments()[0]);

            IdentityEntityTypeNames = identityEntityTypes.Select(o => o.GetNameWithoutGenericParameters());
        }

        public static void ClearCache()
        {
            IdentityDbContextType = null;
            IdentityEntityTypeNames = null;
        }
    }
}
