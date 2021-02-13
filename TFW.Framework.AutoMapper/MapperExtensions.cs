using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public static ET To<ET>(this object obj, IMapper mapper = null)
        {
            mapper = mapper ?? GlobalMapper.Instance;
            CheckMapperNull(mapper);
            return mapper.Map<ET>(obj);
        }

        public static IEnumerable<ET> To<ET>(this IEnumerable<object> obj, IMapper mapper = null)
        {
            mapper = mapper ?? GlobalMapper.Instance;
            CheckMapperNull(mapper);
            return obj.Select(o => o.To<ET>());
        }

        private static void CheckMapperNull(IMapper mapper)
        {
            if (mapper == null)
                throw new ArgumentNullException(nameof(mapper));
        }
    }
}
