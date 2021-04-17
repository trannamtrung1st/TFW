using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using TFW.Framework.Common.Extensions;
using TFW.Framework.Common.Helpers;
using TFW.Framework.EFCore.Migration;
using TFW.Framework.EFCore.Options;
using TFW.Framework.EFCore.Providers;

namespace TFW.Framework.EFCore
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddDefaultDbMigrator(this IServiceCollection services)
        {
            return services.AddSingleton<IDbMigrator>(new DefaultDbMigrator());
        }

        public static IServiceCollection ConfigureGlobalQueryFilter(this IServiceCollection services,
            IEnumerable<Assembly> assemblies,
            params QueryFilter[] filters)
        {
            IQueryFilterProvider[] defaultFilterProviders = null;
            if (assemblies?.Any() == true)
            {
                defaultFilterProviders = ReflectionHelper.GetAllTypesAssignableTo(
                    typeof(IQueryFilterProvider), assemblies)
                        .Select(o => o.CreateInstance<IQueryFilterProvider>())
                        .ToArray();
            }

            return services.Configure<QueryFilterOptions>(opt =>
            {
                if (!defaultFilterProviders.IsNullOrEmpty())
                    foreach (var filter in defaultFilterProviders.SelectMany(p => p.DefaultFilters))
                        opt.ReplaceOrAddFilter(filter);

                foreach (var filter in filters)
                    opt.ReplaceOrAddFilter(filter);
            });
        }
    }
}
