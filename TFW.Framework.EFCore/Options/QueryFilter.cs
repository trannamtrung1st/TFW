using System;
using TFW.Framework.EFCore.Extensions;

namespace TFW.Framework.EFCore.Options
{
    public class QueryFilter
    {
        public string Name { get; set; }
        public bool IsEnabled { get; set; }
        public Func<Type, bool> ApplyFilter { get; set; }

        public QueryFilter() { }

        public QueryFilter(string name, bool isEnabled = true, Func<Type, bool> applyFilter = null)
        {
            Name = name;
            IsEnabled = isEnabled;
            ApplyFilter = applyFilter;
        }

        public QueryFilter Clone()
        {
            return new QueryFilter(Name, IsEnabled, ApplyFilter);
        }

        public static QueryFilter BuildDefaultSoftDelete()
        {
            return new QueryFilter(QueryFilterConsts.SoftDeleteDefaultName,
                isEnabled: true,
                applyFilter: o => o.IsSoftDeleteEntity());
        }
    }
}
