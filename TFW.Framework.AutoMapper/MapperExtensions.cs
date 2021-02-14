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
        public static void CopyFrom(this object obj, object src, IMapper mapper = null)
        {
            mapper = mapper ?? GlobalMapper.Instance;
            CheckMapperNull(mapper);
            mapper.Map(src, obj);
        }

        public static void CopyTo(this object obj, object dest, IMapper mapper = null)
        {
            mapper = mapper ?? GlobalMapper.Instance;
            CheckMapperNull(mapper);
            mapper.Map(obj, dest);
        }

        public static Dest To<Dest>(this object obj, IMapper mapper = null)
        {
            mapper = mapper ?? GlobalMapper.Instance;
            CheckMapperNull(mapper);
            return mapper.Map<Dest>(obj);
        }

        public static IEnumerable<Dest> To<Dest>(this IEnumerable<object> obj, IMapper mapper = null)
        {
            mapper = mapper ?? GlobalMapper.Instance;
            CheckMapperNull(mapper);
            return obj.Select(o => o.To<Dest>());
        }

        private static void CheckMapperNull(IMapper mapper)
        {
            if (mapper == null)
                throw new ArgumentNullException(nameof(mapper));
        }
    }
}
