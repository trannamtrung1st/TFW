using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using TFW.Framework.Cross.Models;

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
                applyFilter: o => typeof(ISoftDeleteEntity).IsAssignableFrom(o));
        }
    }

    public class QueryFilterOptions
    {
        private IDictionary<string, QueryFilter> _filterMap;
        public IReadOnlyDictionary<string, QueryFilter> FilterMap => _filterMap.ToImmutableDictionary();

        public QueryFilterOptions()
        {
            _filterMap = new Dictionary<string, QueryFilter>();
        }

        public QueryFilterOptions EnableFilter(string filterName)
        {
            _filterMap[filterName].IsEnabled = true;

            return this;
        }

        public QueryFilterOptions DisableFilter(string filterName)
        {
            _filterMap[filterName].IsEnabled = false;

            return this;
        }

        public QueryFilterOptions ReplaceOrAddFilter(QueryFilter filter)
        {
            _filterMap[filter.Name] = filter;

            return this;
        }

        public bool IsEnabled(string filterName)
        {
            return _filterMap.ContainsKey(filterName) && _filterMap[filterName].IsEnabled;
        }

        public bool IsAppliedForEntity(string filterName, Type eType)
        {
            return _filterMap.ContainsKey(filterName) && (_filterMap[filterName].ApplyFilter == null
                || _filterMap[filterName].ApplyFilter(eType));
        }

        public bool IsEnabledAndAppliedForEntity(string filterName, Type eType)
        {
            return IsEnabled(filterName) && IsAppliedForEntity(filterName, eType);
        }
    }
}
