using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace TFW.Framework.AutoMapper
{
    public static class MapperExtensions
    {
        public static void CopyFrom(this IMapper mapper, object dest, object src)
        {
            CheckMapperNull(mapper);
            mapper.Map(src, dest);
        }

        public static void CopyTo(this IMapper mapper, object src, object dest)
        {
            CheckMapperNull(mapper);
            mapper.Map(src, dest);
        }

        public static Dest MapTo<Dest>(this IMapper mapper, object src)
        {
            CheckMapperNull(mapper);
            return mapper.Map<Dest>(src);
        }

        public static IEnumerable<Dest> MapTo<Dest>(this IMapper mapper, IEnumerable<object> src)
        {
            CheckMapperNull(mapper);
            return src.Select(o => mapper.MapTo<Dest>(o));
        }

        private static void CheckMapperNull(IMapper mapper)
        {
            if (mapper == null)
                throw new ArgumentNullException(nameof(mapper));
        }
    }
}
