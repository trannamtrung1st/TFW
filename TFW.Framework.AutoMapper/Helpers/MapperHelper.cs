using AutoMapper;
using AutoMapper.QueryableExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace TFW.Framework.AutoMapper.Helpers
{
    public static class MapperHelper
    {
        public static void CopyFrom(this object dest, object src, IMapper mapper = null)
        {
            if (mapper == null) mapper = GlobalMapper.Instance;

            CheckMapperNull(mapper);

            mapper.Map(src, dest);
        }

        public static void CopyTo(this object src, object dest, IMapper mapper = null)
        {
            if (mapper == null) mapper = GlobalMapper.Instance;

            CheckMapperNull(mapper);

            mapper.Map(src, dest);
        }

        public static Dest MapTo<Dest>(this object src, IMapper mapper = null)
        {
            if (mapper == null) mapper = GlobalMapper.Instance;

            CheckMapperNull(mapper);

            return mapper.Map<Dest>(src);
        }

        public static IEnumerable<Dest> MapTo<Dest>(this IEnumerable<object> src, IMapper mapper = null)
        {
            if (mapper == null) mapper = GlobalMapper.Instance;

            CheckMapperNull(mapper);

            return src.Select(o => o.MapTo<Dest>());
        }

        public static IQueryable<T> DefaultProjectTo<T>(this IQueryable query, IConfigurationProvider configurationProvider = null)
        {
            if (configurationProvider == null) configurationProvider = GlobalMapper.Instance?.ConfigurationProvider;

            return query.ProjectTo<T>(configurationProvider);
        }

        private static void CheckMapperNull(IMapper mapper)
        {
            if (mapper == null)
                throw new ArgumentNullException(nameof(mapper));
        }
    }
}
