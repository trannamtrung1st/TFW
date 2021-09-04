using AutoMapper;
using System;

namespace TFW.Framework.AutoMapper
{
    public static class GlobalMapper
    {
        private static IMapper _instance;
        public static IMapper Instance => _instance;

        public static void Init(IMapper mapper)
        {
            if (_instance != null)
                throw new InvalidOperationException($"Already initialized {nameof(IMapper)}");

            _instance = mapper;
        }
    }
}
