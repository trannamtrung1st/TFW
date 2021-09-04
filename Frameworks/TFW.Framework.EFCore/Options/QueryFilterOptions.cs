using System;
using System.Collections.Generic;

namespace TFW.Framework.EFCore.Options
{
    public class QueryFilterOptions
    {
        private Dictionary<string, QueryFilter> _filterMap;
        public IReadOnlyDictionary<string, QueryFilter> FilterMap => _filterMap;

        public QueryFilterOptions()
        {
            _filterMap = new Dictionary<string, QueryFilter>();
        }

        public QueryFilterOptions EnableFilter(params string[] filterNames)
        {
            foreach (var name in filterNames)
                _filterMap[name].IsEnabled = true;

            return this;
        }

        public QueryFilterOptions DisableFilter(params string[] filterNames)
        {
            foreach (var name in filterNames)
                _filterMap[name].IsEnabled = false;

            return this;
        }

        public QueryFilterOptions ReplaceOrAddFilter(params QueryFilter[] filters)
        {
            foreach (var filter in filters)
                _filterMap[filter.Name] = filter;

            return this;
        }

        public bool IsEnabled(string filterName)
        {
            return _filterMap.ContainsKey(filterName) && _filterMap[filterName].IsEnabled;
        }

        public bool IsAppliedForEntity(string filterName, Type eType)
        {
            return _filterMap.ContainsKey(filterName) && (_filterMap[filterName].IsEnabled
                && (_filterMap[filterName].ApplyFilter == null
                || _filterMap[filterName].ApplyFilter(eType)));
        }
    }
}
